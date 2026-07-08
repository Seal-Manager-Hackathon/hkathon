# Công thức tính điểm & Context cho từng tầng (dùng cho AI Agent)

## Sơ đồ phân cấp

```
CriteriaItem (tiêu chí — điểm tối đa trong field Score)
   └─ chấm bởi nhiều Judge → ScoreItems (mỗi Judge tạo 1 bản chấm)
        └─ gộp lại → Scores.TotalScore (điểm Submission trong Round)
             └─ nhiều Round → EventScore (điểm Team trong Event)
                  └─ nhiều Event trong năm → ChapterScore (điểm Team trong năm)
```

---

## Tầng 1: CriteriaItem → Score (điểm 1 Submission trong 1 Round)

### Công thức

```
criteriaAvg = AVG(ScoreItems.Score) GROUP BY CriteriaItemId
               └─ nhiều judge chấm cùng 1 tiêu chí → lấy trung bình

Scores.TotalScore = SUM(criteriaAvg)   với mọi CriteriaItemId thuộc template của round
```

### Entity mapping

| Entity field | Vai trò |
|---|---|
| `CriteriaItems.Score` | Điểm tối đa cho tiêu chí đó (chỉ để hiển thị, ko ảnh hưởng tính toán) |
| `ScoreItems.Score` (decimal?) | Điểm judge chấm cho tiêu chí đó |
| `ScoreItems.CriteriaItemId` | FK → CriteriaItems |
| `Scores.TotalScore` (decimal) | Kết quả sau tính toán, lưu vào DB |

### Code

```csharp
// RoundScoreHelper.CalculateScoreTotal(scoreItems)
// Group by CriteriaItemId → avg each group → sum all avgs
var grouped = scoreItems.GroupBy(si => si.CriteriaItemId);
foreach (var group in grouped)
    total += group.Average(si => si.Score!.Value);
```

### Context

- **AVG giữa các judge**: công bằng, tránh 1 judge chấm quá khắt khe/dễ dãi.
- **SUM giữa các tiêu chí**: mỗi tiêu chí đánh giá 1 khía cạnh độc lập, cộng đủ mới phản ánh đúng năng lực toàn diện.
- **Ràng buộc**: chỉ tính trên **submission cuối cùng** của team trong round đó. Judge chỉ được chấm nếu được phân công vào đúng Track của team.

---

## Tầng 2: Round → Event (điểm 1 Team trong 1 Event)

### Công thức

```
eventScore(team, event) = SUM(roundScore_i)
   với i chạy qua các round team ĐÃ THAM GIA
```

### Entity mapping

| Entity field | Vai trò |
|---|---|
| `Scores.TotalScore` | Điểm 1 round (đã tính ở tầng 1) |
| `Rounds.RoundNo` | Số thứ tự round — round team đã qua (qua RoundDetails) |
| `RoundDetails` | Bảng junction: RegisterTeams × Rounds |

Không có `Weight` trên Round entity → không thể dùng weighted average.

### Tại sao dùng Sum thay vì Average?

| Team | Round 1 | Round 2 | Event (Sum) | Event (Average) |
|---|---|---|---|---|
| A (vào sâu) | 80 | 70 | **150** | 75 |
| B (dừng sớm) | 80 | — | **80** | 80 |

- **Sum**: team vô sâu hơn → tích lũy nhiều điểm hơn ✅ phản ánh đúng nỗ lực.
- **Average**: team dừng sớm vẫn có thể ngang hoặc cao hơn ❌ vô lý.

Trong cùng 1 event, các team đều có cơ hội như nhau để qua các round, nên Sum là công bằng.

### Code

```csharp
// EventScoreHelper.Calculate(list of round scores)
// Sum of all round scores for this event
return Math.Round(roundScores.Sum(), 2);
```

---

## Tầng 3: Event → Chapter (điểm 1 Team trong 1 năm)

### Công thức

```
chapterScore(team, year) = AVG(eventScore(team, event_j))
   với j chạy qua các event team đã tham gia trong năm đó
```

### Tại sao dùng Average thay vì Sum?

Các event có số round khác nhau → Sum event scores sẽ thiệt cho event ít round:

| Team | Event A (3 rounds, sum=240) | Event B (2 rounds, sum=165) | Chapter (Sum) | Chapter (Average) |
|---|---|---|---|---|
| Tham gia A+B | 240 | 165 | **405** | **202.5** |
| Chỉ tham gia A | 240 | — | **240** | **240** |

Nếu dùng Sum, team tham gia nhiều event hơn sẽ luôn có chapter cao hơn — không phản ánh đúng chất lượng.

**Average** chuẩn hóa giữa các event có cấu trúc khác nhau, chỉ đo chất lượng trung bình mỗi event.

### Code

```csharp
// ChapterScoreHelper.Calculate(list of event scores)
// Average across events to normalize for different round counts
return Math.Round(eventScores.Average(), 2);
```

---

## Bảng tổng hợp nhanh

| Tầng | Input | Phép toán | Output | Ghi chú |
|---|---|---|---|---|
| CriteriaItem → Score | Điểm judge chấm từng criteria | AVG theo criteria, SUM các criteria | `Scores.TotalScore` | Submission cuối cùng; judge đúng track |
| Round → Event | `TotalScore` các round | SUM | `eventScore` | Entity ko có Weight → ko weighted avg |
| Event → Chapter | `eventScore` các event trong năm | AVG | `chapterScore` | Chuẩn hóa giữa event có số round khác nhau |

## Ghi chú nghiệp vụ

- 1 Team phải tồn tại trước khi đăng ký tham gia Event.
- Khi được duyệt tham gia event → team chọn Track + bốc thăm Topic → staff gán Track/Topic vào `RegisterTeam`.
- Mỗi Round có đúng 1 `CriteriaTemplate` áp dụng chung cho mọi Track/Topic trong round đó.
- Chỉ leader được nộp bài; team và judge chỉ thấy bản nộp cuối cùng trong round; staff (được phân công vào event) và admin thấy toàn bộ lịch sử nộp bài.
- Judge chỉ chấm được submission thuộc Track mà mình được phân công.
