# Công thức tính điểm — Glossary & Cách tính

## Glossary (thuật ngữ chuẩn dùng trong code & doc)

| Thuật ngữ | Entity / Field | Ý nghĩa |
|---|---|---|
| **judgeScore** | `ScoreItems.Score` (decimal?) | Điểm 1 judge chấm cho 1 tiêu chí (criteria item) của 1 submission. Mỗi judge tạo 1 ScoreItem. |
| **criteriaAvg** | — (tính toán) | Trung bình các judgeScore có cùng `CriteriaItemId`. Là điểm cuối của tiêu chí đó cho submission. |
| **scopeScore** | `Scores.TotalScore` (decimal) | Tổng điểm 1 submission = SUM các criteriaAvg. Lưu vào DB. |
| **eventScore** | — (tính toán) | Điểm của 1 team trong 1 event. Weighted average theo round. |
| **chapterScore** | — (tính toán) | Điểm của 1 team trong 1 năm. AVG các eventScore. |

---

## Tầng 1: CriteriaItem → scopeScore (điểm 1 Submission trong 1 Round)

### Công thức

```
judgeScore_i(k) = ScoreItems.Score   (judge thứ i chấm cho criteria item k)

criteriaAvg(k) = AVG(judgeScore_i(k))   với mọi judge i đã chấm (có ScoreItem tồn tại)
                                          └─ CHỈ tính judge đã chấm thực tế
                                          └─ ko yêu cầu tất cả judge được phân công
                                              → phòng judge bận ko chấm

scopeScore = SUM(criteriaAvg(k))   với mọi criteria item k trong template của round
```

### Entity mapping

| Field | Vai trò |
|---|---|
| `CriteriaItems.Score` | Điểm tối đa cho tiêu chí (chỉ để hiển thị, ko ảnh hưởng tính toán) |
| `ScoreItems.Score` (decimal?) | **judgeScore** — điểm judge chấm |
| `ScoreItems.CriteriaItemId` | FK → CriteriaItems, dùng để group tính **criteriaAvg** |
| `ScoreItems.JudgeId` | FK → Users (Judge), để biết judge nào chấm |
| `Scores.TotalScore` (decimal) | **scopeScore** — kết quả sau tính toán, lưu vào DB |

### Code (RoundScoreHelper — ko đổi logic)

```csharp
// scopeScore = SUM(GROUP_AVG(judgeScore, CriteriaItemId))
// Chỉ tính judgeScore có Score.HasValue = true
var validItems = scoreItems.Where(si => si.Score.HasValue);
var grouped = validItems.GroupBy(si => si.CriteriaItemId);

foreach (var group in grouped)
    total += group.Average(si => si.Score!.Value);  // criteriaAvg
```

### Count rules

- **Chỉ đếm judge đã chấm thực tế** (có `ScoreItem.Score != null`).
- Judge được phân công nhưng ko chấm → ko có ScoreItem → ko ảnh hưởng.
- **Ko yêu cầu tất cả judge phải chấm** — phòng judge bận.

---

## Tầng 2: Round → eventScore (điểm 1 Team trong 1 Event)

### Công thức

```
eventScore = Σ(weight_i × roundScore_i) / Σ(weight_i)

Trong đó:
  weight_i     = trọng số round thứ i (mặc định = 1)
  roundScore_i = scopeScore của team trong round thứ i
                 (bằng 0 nếu team KO tham gia round đó)
```

### Entity mapping

| Field | Vai trò |
|---|---|
| `Scores.TotalScore` (decimal) | **roundScore** — scopeScore của submission cuối team trong round |
| `RoundDetails` | Bảng junction: RegisterTeams × Rounds — biết team đã tham gia round nào |
| `Rounds.RoundNo` | Số thứ tự round trong event |
| `Rounds` | Hiện tại **ko có Weight field** → `weight_i = 1` cho tất cả |

### Weighted average — giải thích

Entity `Round` hiện tại **không có** `Weight` hay `RoundWeight` property.  
`weight_i = 1` là giá trị mặc định — khi nào entity có Weight field thì đổi thành giá trị thật.

**Config mặc định** (weight_i = 1 cho tất cả round):

```
eventScore = Σ(roundScore_i) / totalRounds
```

→ Công thức trở thành **average thường**, nhưng vẫn giữ nguyên tắc
**"không tham gia = 0 trong mẫu số"**: mẫu số tính trên tổng số round của event,
kể cả round team ko tham gia (roundScore = 0).

### Vì sao dùng weighted average thay vì Sum?

| Team | R1 | R2 | R3 | Sum | Avg (weight=1) |
|---|---|---|---|---|---|
| A (vào chung kết) | 80 | 75 | 70 | **225** | **75.0** |
| B (dừng R2) | 80 | 75 | — | **155** | **51.7** |
| C (dừng R1) | 80 | — | — | **80** | **26.7** |

- **Sum**: team vào sâu hơn luôn có điểm cao tuyệt đối ✅
- **Weighted Average (weight=1)**: team vào sâu vẫn hơn hẳn, nhưng chuẩn hóa
  theo số round ✅, tránh event có nhiều round quá lấn át chapter.

Đây là **compromise** giữa Sum (thưởng độ sâu) và Average thuần (công bằng):
vì mẫu số tính trên tổng số round của event, team dừng sớm bị 0 ở các round sau
→ điểm thấp hơn hẳn team đi tiếp.

### Code (EventScoreHelper — signature đã đổi)

```csharp
// eventScore = Σ(1 × roundScore_i) / totalRounds
public static decimal Calculate(List<decimal> roundScores, int totalRounds)
{
    var sum = roundScores.Sum();                    // Σ(roundScore_i)
    return Math.Round(sum / totalRounds, 2);        // ÷ tổng số round
}
```

---

## Tầng 3: Event → chapterScore (điểm 1 Team trong 1 năm)

### Công thức

```
chapterScore = AVG(eventScore_j)
   với j chạy qua các event team đã tham gia trong năm đó
```

### Entity mapping

| Field | Vai trò |
|---|---|
| `RegisterEvents` | Bảng junction: RegisterTeams × Events — biết team tham gia event nào trong năm |
| `Scores.TotalScore` | scopeScore dùng tính eventScore cho từng event |

### Tại sao dùng Average thay vì Sum?

| Team | Event A (3 rounds) | Event B (2 rounds) | Sum | Avg |
|---|---|---|---|---|
| Tham gia A+B | 75.0 | 82.5 | **157.5** | **78.8** |
| Chỉ tham gia A | 75.0 | — | **75.0** | **75.0** |

- **Sum**: team tham gia nhiều event hơn sẽ luôn cao hơn — ko phản ánh chất lượng.
- **Average**: chuẩn hóa giữa các event có cấu trúc (số round, weight) khác nhau,
  chỉ đo chất lượng trung bình mỗi event.

### Code (ChapterScoreHelper — ko đổi)

```csharp
public static decimal Calculate(List<decimal> eventScores)
{
    return Math.Round(eventScores.Average(), 2);
}
```

---

## Bảng tổng hợp nhanh

| Tầng | Input | Phép toán | Output | Ghi chú |
|---|---|---|---|---|
| CriteriaItem → scopeScore | judgeScore (`ScoreItems.Score`) | AVG theo CriteriaItemId → SUM | `Scores.TotalScore` | Chỉ tính judge đã chấm thực tế |
| Round → eventScore | scopeScore các round | Weighted AVG (weight_i = 1) | eventScore | Mẫu số = tổng số round event |
| Event → chapterScore | eventScore các event trong năm | AVG | chapterScore | Chuẩn hóa số round khác nhau |

## Ghi chú nghiệp vụ

- 1 Team phải tồn tại trước khi đăng ký tham gia Event.
- Khi được duyệt tham gia event → team chọn Track + bốc thăm Topic → staff gán Track/Topic vào `RegisterTeam`.
- Mỗi Round có đúng 1 `CriteriaTemplate` áp dụng chung cho mọi Track/Topic trong round đó.
- Chỉ leader được nộp bài; team và judge chỉ thấy bản nộp cuối cùng trong round; staff (được phân công vào event) và admin thấy toàn bộ lịch sử nộp bài.
- Judge chỉ chấm được submission thuộc Track mà mình được phân công.
- **Ko có `Weight` trên Round entity hiện tại** — mọi weight mặc định = 1. Khi thêm Weight field, chỉ cần thay đổi code `EventScoreHelper`.
