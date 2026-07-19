# Time Check — Commented để dễ test

> Các check thời gian đã được **giảm nhẹ**: chỉ giữ lại ràng buộc cơ bản, comment các ràng buộc nặng.
> Các ràng buộc đã bị comment có tag `[Commented]`.
> Khi cần bật lại: search `[Commented]` trong các file → xóa `//` hoặc mở comment block.
>
> ✅ = check còn giữ lại (active)
> 🟡 = check cơ bản mới uncomment
> `[Commented]` = check đã comment

---

## I. Admin — Event

### 1. CreateEvent
**API:** `POST /api/v1/admin/events`
**File:** `Hackathon.Application/Services/Admin/Event/Service.cs`
**Hàm:** `CreateEvent`

**🟡 Check active (vừa uncomment):**
```csharp
// EndTime phải > StartTime (event có thời gian kết thúc sau thời gian bắt đầu)
if (request.EndTime <= request.StartTime)
    throw new BadRequestException(ErrMsg.Event.EndTimeMustBeAfterStartTime);
```

**Check bị comment:**
```csharp
// [Commented] RegisterLimitTime check — bỏ để dễ test
//if (request.RegisterLimitTime.HasValue)
//{
//    if (request.RegisterLimitTime.Value <= request.StartTime)
//        throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeAfterStartTime);
//    if (request.RegisterLimitTime.Value >= request.EndTime)
//        throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeBeforeEndTime);
//}
```

### 2. UpdateEvent
**API:** `PATCH /api/v1/admin/events/{eventId}`
**File:** `Hackathon.Application/Services/Admin/Event/Service.cs`
**Hàm:** `UpdateEvent`

**🟡 Check active (vừa uncomment):**
```csharp
// EndTime phải > StartTime (chỉ check nếu có cập nhật thời gian)
var startTime = request.StartTime ?? ev.StartTime;
var endTime = request.EndTime ?? ev.EndTime;
if (endTime <= startTime && (request.EndTime.HasValue || request.StartTime.HasValue))
    throw new BadRequestException(ErrMsg.Event.EndTimeMustBeAfterStartTime);
```

**Check bị comment:**
```csharp
// [Commented] StartTime phải > hiện tại — bỏ để dễ test
//if (startTime <= now && request.StartTime.HasValue)
//    throw new BadRequestException(ErrMsg.Event.StartTimeMustBeAfterNow);

// [Commented] RegisterLimitTime check — bỏ để dễ test
//var registerLimitTime = request.RegisterLimitTime ?? ev.RegisterLimitTime;
//if (registerLimitTime.HasValue)
//{
//    if (registerLimitTime.Value <= startTime && request.RegisterLimitTime.HasValue)
//        throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeAfterStartTime);
//    if (registerLimitTime.Value >= endTime && request.RegisterLimitTime.HasValue)
//        throw new BadRequestException(ErrMsg.Event.RegisterLimitTimeMustBeBeforeEndTime);
//}
```

---

## II. Admin — Round

### 3. CreateRound
**API:** `POST /api/v1/admin/rounds`
**File:** `Hackathon.Application/Services/Admin/Round/Service.cs`
**Hàm:** `CreateRound`

**🟡 Check active (vừa uncomment):**
```csharp
// EndTime > StartTime (round có thời gian kết thúc sau khi bắt đầu)
if (request.EndTime <= request.StartTime)
    throw new BadRequestException(ErrMsg.Round.EndTimeMustBeAfterStartTime);
```

**✅ Giữ lại từ trước:**
```csharp
// Check StartTime và EndTime của round phải nằm trong event time — giữ lại
if (ev.StartTime.HasValue && request.StartTime < ev.StartTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
if (ev.EndTime.HasValue && request.EndTime > ev.EndTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);

// Check previous round: StartTime >= EndTime của round trước — giữ lại
if (newRoundNo > 1)
{
    var prevRound = await _roundRepository.GetByEventIdAndRoundNoAsync(request.EventId, newRoundNo - 1);
    if (prevRound?.EndTime.HasValue == true && request.StartTime < prevRound.EndTime.Value)
        throw new BadRequestException(ErrMsg.Round.RoundStartTimeMustBeAfterPreviousRoundEndTime);
}
```

**Check bị comment:**
```csharp
// [Commented] StartSubmission/EndSubmission check — bỏ để dễ test
// if (request.StartSubmission.HasValue && request.StartSubmission.Value < request.StartTime)
//     throw new BadRequestException(...);
// if (request.EndSubmission.HasValue && request.EndSubmission.Value > request.EndTime)
//     throw new BadRequestException(...);
// if (request.LimitTeam.HasValue && request.LimitTeam.Value < 1)
//     throw new BadRequestException(...);

// [Commented] StartTime >= RegisterLimitTime của event
// if (ev.RegisterLimitTime.HasValue && request.StartTime < ev.RegisterLimitTime.Value)
//     throw new BadRequestException(...);
```

### 4. UpdateRound
**API:** `PATCH /api/v1/admin/rounds/{roundId}`
**File:** `Hackathon.Application/Services/Admin/Round/Service.cs`
**Hàm:** `UpdateRound`

**🟡 Check active (vừa uncomment):**
```csharp
// EndTime > StartTime (round có thời gian kết thúc sau khi bắt đầu)
if (!startTime.HasValue || !endTime.HasValue)
    throw new BadRequestException("Start Time And End Time Are Required");
if (endTime.Value <= startTime.Value)
    throw new BadRequestException(ErrMsg.Round.EndTimeMustBeAfterStartTime);
```

**✅ Giữ lại từ trước:**
```csharp
// Round time must be within event time — giữ lại
if (ev.StartTime.HasValue && startTime.Value < ev.StartTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
if (ev.EndTime.HasValue && endTime.Value > ev.EndTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
```

**🟡 Vừa uncomment:**
```csharp
// Check previous round (RoundNo - 1): StartTime >= EndTime của round trước
if (round.RoundNo.HasValue && round.RoundNo.Value > 1)
{
    var prevRound = await _roundRepository.GetByEventIdAndRoundNoAsync(round.EventId, round.RoundNo.Value - 1);
    if (prevRound?.EndTime.HasValue == true && startTime.Value < prevRound.EndTime.Value)
        throw new BadRequestException(ErrMsg.Round.RoundStartTimeMustBeAfterPreviousRoundEndTime);
}
```

**Check bị comment:**
```csharp
// [Commented] LimitTeam >= 1 — bỏ để dễ test
// if (limitTeam.HasValue && limitTeam.Value < 1)
//     throw new BadRequestException(...);

// [Commented] StartTime >= RegisterLimitTime của event — bỏ để dễ test
//if (ev.RegisterLimitTime.HasValue && startTime.Value < ev.RegisterLimitTime.Value)
//    throw new BadRequestException(...);

// [Commented] Check next round — bỏ để dễ test
// if (round.RoundNo.HasValue)
// {
//     var nextRound = await _roundRepository.GetByEventIdAndRoundNoAsync(round.EventId, round.RoundNo.Value + 1);
//     if (nextRound?.StartTime.HasValue == true && endTime.Value > nextRound.StartTime.Value)
//         throw new BadRequestException(...);
// }
```

### 5. SwapRound
**API:** `POST /api/v1/admin/rounds/{roundId}/swap`
**File:** `Hackathon.Application/Services/Admin/Round/Service.cs`
**Hàm:** `SwapRound`
> **Không thay đổi** — vẫn comment như cũ (event start check)

**Check bị comment:**
```csharp
// [Commented] Ko cho swap sau khi event da start — bỏ check để dễ test
//if (ev.StartTime.HasValue && DateTimeOffset.UtcNow >= ev.StartTime.Value)
//    throw new BadRequestException("Cannot Swap Round After Event Has Started");
```

### 6. EndRound
**API:** `POST /api/v1/admin/rounds/{roundId}/end`
**File:** `Hackathon.Application/Services/Admin/Round/Service.cs`
**Hàm:** `EndRound`
> **Không thay đổi** — vẫn comment như cũ

**Check bị comment:**
```csharp
// [Commented] EndRound checks — bỏ check để dễ test
//if (round.IsDisable)
//    throw new BadRequestException("Cannot End A Disabled Round");
//if (!round.StartTime.HasValue || round.StartTime.Value > DateTimeOffset.UtcNow)
//    throw new BadRequestException("Round Cannot Be Ended Before It Starts");
//if (round.EndTime.HasValue && round.EndTime.Value <= DateTimeOffset.UtcNow)
//    throw new BadRequestException("Round Has Already Ended");
```

---

## III. Admin — RegisterTeam

> **Không thay đổi** — tất cả check thời gian đã comment từ trước, giữ nguyên.

### 7. ApproveRegisterTeam
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/approve`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `ApproveRegisterTeam`
**Check bị comment:** Event time window, Round1 start check (giữ nguyên)

### 8. RejectRegisterTeam
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/reject`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `RejectRegisterTeam`
**Check bị comment:** Event time window (giữ nguyên)

### 9. AssignToNextRound
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/assign-next-round`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `AssignToNextRound`
**Check bị comment:** Event ended check (giữ nguyên)

### 10. RevertToPreviousRound
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/revert-previous-round`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `RevertToPreviousRound`
**Check bị comment:** Event ended check (giữ nguyên)

---

## IV. Các Admin services KHÔNG có ràng buộc thời gian

Các service sau đã được kiểm tra và **không có bất kỳ check thời gian nào** (không cần động):
| Service | File |
|---------|------|
| Award | `Admin/Award/Service.cs` |
| Track | `Admin/Track/Service.cs` |
| Topic | `Admin/Topic/Service.cs` |
| User | `Admin/User/Service.cs` |
| Team | `Admin/Team/Service.cs` |
| Invitation | `Admin/Invitation/Service.cs` |
| Notification | `Admin/Notification/Service.cs` |
| Assign | `Admin/Assign/Service.cs` |
| Report | `Admin/Report/Service.cs` |
| Score | `Admin/Score/Service.cs` |
| Submission | `Admin/Submission/Service.cs` |
| Leaderboard | `Admin/Leaderboard/Service.cs` |
| CriteriaTemplate | `Admin/CriteriaTemplate/Service.cs` |

---

## V. Staff — RegisterTeam

> Giống Admin mục 7-10. **Chưa check/sửa lần này**

---

## VI. Judge

> **Không cần sửa** — tất cả check thời gian đã được comment từ trước:
> - `SubmitScore`: round.EndTime + event.EndTime check ✅ đã comment
> - `UpdateScore`: event ended check ✅ đã comment
> - `UpdateScoreItem`: event ended check ✅ đã comment

---

## VII. Lecturer

> **Không có ràng buộc thời gian nào** — Lecturer chỉ đọc dữ liệu (Event, Round, RegisterTeam, Submission, Score, ...)
>
> **Ngoại lệ:** `GetCompetitionStatus` dùng `DateTimeOffset.UtcNow` để xác định round hiện tại (business logic xem team còn đang thi đấu ko, không phải validation)

---

## VIII. Student

### 18. CreateRegisterTeam
**API:** `POST /api/v1/student/register-events`
**File:** `Hackathon.Application/Services/Student/RegisterTeam/Service.cs`
**Hàm:** `CreateRegisterTeam`
> **Không thay đổi** — đã comment `RegisterLimitTime` + `StartTime` từ trước

**Check bị comment:**
```csharp
// [Commented] Check registration is within the allowed time window — bỏ check để dễ test
//if (ev.RegisterLimitTime.HasValue && DateTimeOffset.UtcNow >= ev.RegisterLimitTime.Value)
//    throw new BadRequestException("Registration Period Has Ended. Cannot Register At This Time.");
//if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
//    throw new BadRequestException("Registration Has Not Started Yet. Cannot Register Before Event Starts.");
```

**✅ Giữ lại từ trước:**
```csharp
if (ev.Status == EventStatusEnum.Draft || ev.Status == EventStatusEnum.Closed)
    throw new BadRequestException("Cannot Register to a Draft or Closed Event");
```

### 19. CreateSubmission
**API:** `POST /api/v1/student/submissions`
**File:** `Hackathon.Application/Services/Student/Submission/Service.cs`
**Hàm:** `CreateSubmission`

**Check vừa comment (lần này):**
```csharp
// [Commented] Check submission time window — bỏ để dễ test
//var now = DateTimeOffset.UtcNow;
//if (round.StartSubmission.HasValue && now < round.StartSubmission.Value)
//    throw new BadRequestException("Submission Period Has Not Started Yet");
//if (round.EndSubmission.HasValue && now > round.EndSubmission.Value)
//    throw new BadRequestException("Submission Period Has Ended");
```

### 20. Các Student services khác — không có ràng buộc thời gian
| Service | File |
|---------|------|
| Event | `Student/Event/Service.cs` |
| Round | `Student/Round/Service.cs` |
| Award | `Student/Award/Service.cs` |
| Track | `Student/Track/Service.cs` |
| Topic | `Student/Topic/Service.cs` |
| Team | `Student/Team/Service.cs` |
| User | `Student/User/Service.cs` |
| Invitation | `Student/Invitation/Service.cs` |
| Notification | `Student/Notification/Service.cs` |
| Assign | `Student/Assign/Service.cs` |
| Report | `Student/Report/Service.cs` |
| Leaderboard | `Student/Leaderboard/Service.cs` |
| CriteriaTemplate | `Student/CriteriaTemplate/Service.cs` |
| PopularEvent | `Student/PopularEvent/Service.cs` |

---

## VIII. Tổng kết thay đổi

| # | API | Trước | Sau | Ghi chú |
|---|-----|-------|-----|---------|
| 1 | Admin POST CreateEvent | ❌ Commented | 🟡 EndTime > StartTime | RegisterLimitTime vẫn comment |
| 2 | Admin PATCH UpdateEvent | ❌ Commented | 🟡 EndTime > StartTime | StartTime>now + RegisterLimitTime vẫn comment |
| 3 | Admin POST CreateRound | ❌ Commented | 🟡 EndTime > StartTime | Submission/LimitTeam/RegisterLimit vẫn comment |
| 4 | Admin PATCH UpdateRound | ❌ Commented | 🟡 EndTime > StartTime + 🟡 Previous round check | Next round/LimitTeam/RegisterLimit vẫn comment |
| 5 | Admin POST SwapRound | ❌ Commented | ❌ Commented | Không đổi |
| 6 | Admin POST EndRound | ❌ Commented | ❌ Commented | Không đổi |
| 7-10 | Admin RegisterTeam các API | ❌ Commented | ❌ Commented | Không đổi |
| 18 | Student POST CreateRegisterTeam | ❌ Commented | ❌ Commented | Đã comment từ trước |
| 19 | Student POST CreateSubmission | ✅ Active | ❌ Commented | **Vừa comment** StartSubmission/EndSubmission |

> 🟡 = vừa uncomment (active), ❌ = commented, ✅ = đã active từ trước

## Cách bật lại

1. Mở file tương ứng
2. Search `[Commented]` — mỗi block comment đều có tag này
3. Xóa `//` hoặc mở comment block
4. Build lại: `dotnet build`
