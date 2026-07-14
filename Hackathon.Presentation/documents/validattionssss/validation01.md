# Time Check — Commented để dễ test

> Các check thời gian đã bị comment `// [Commented]` tạm thời để dễ test.
> Khi cần bật lại: search `[Commented]` trong các file → xóa `//`.
>
> ✅ = check còn giữ lại (ko comment)

---

## I. Admin — Event

### 1. CreateEvent
**API:** `POST /api/v1/admin/events`
**File:** `Hackathon.Application/Services/Admin/Event/Service.cs`
**Hàm:** `CreateEvent`

**Check bị comment:**
```csharp
// [Commented] Check thời gian CreateEvent — bỏ check để dễ test
//if (request.EndTime <= request.StartTime)
//    throw new BadRequestException(ErrMsg.Event.EndTimeMustBeAfterStartTime);
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

**Check bị comment:**
```csharp
// [Commented] Validate thời gian UpdateEvent — bỏ check để dễ test
//var startTime = request.StartTime ?? ev.StartTime;
//var endTime = request.EndTime ?? ev.EndTime;
//var registerLimitTime = request.RegisterLimitTime ?? ev.RegisterLimitTime;
//if (startTime <= now && request.StartTime.HasValue)
//    throw new BadRequestException(ErrMsg.Event.StartTimeMustBeAfterNow);
//if (endTime <= startTime && (request.EndTime.HasValue || request.StartTime.HasValue))
//    throw new BadRequestException(ErrMsg.Event.EndTimeMustBeAfterStartTime);
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

**Check bị comment:**
```csharp
// Đã comment từ trước (ko phải do session này):
// - EndTime <= StartTime
// - StartSubmission / EndSubmission validation
// - LimitTeam >= 1
// - RegisterLimitTime check
```

**✅ Giữ lại:**
```csharp
// Check StartTime và EndTime của round phải nằm trong event time — giữ lại
if (ev.StartTime.HasValue && request.StartTime < ev.StartTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
if (ev.EndTime.HasValue && request.EndTime > ev.EndTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
```

### 4. UpdateRound
**API:** `PATCH /api/v1/admin/rounds/{roundId}`
**File:** `Hackathon.Application/Services/Admin/Round/Service.cs`
**Hàm:** `UpdateRound`

**Check bị comment:**
```csharp
// Đã comment từ trước (ko phải do session này):
// - Start/End time required
// - EndTime <= StartTime
// - LimitTeam >= 1
// - RegisterLimitTime check
// - Previous round time overlap check
// - Next round time overlap check
```

**✅ Giữ lại:**
```csharp
// Round time must be within event time — giữ lại
if (ev.StartTime.HasValue && startTime.Value < ev.StartTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
if (ev.EndTime.HasValue && endTime.Value > ev.EndTime.Value)
    throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);
```

### 5. SwapRound
**API:** `POST /api/v1/admin/rounds/{roundId}/swap`
**File:** `Hackathon.Application/Services/Admin/Round/Service.cs`
**Hàm:** `SwapRound`

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

### 7. ApproveRegisterTeam
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/approve`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `ApproveRegisterTeam`

**Check bị comment:**
```csharp
// [Commented] Check event time window — bỏ check để dễ test
//var ev = await _eventRepository.GetByIdAsync(rt.EventId);
//if (ev != null)
//{
//    if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
//        throw new BadRequestException("Cannot Approve Before Event Starts");
//    if (ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
//        throw new BadRequestException("Cannot Approve After Event Has Ended");
//}

// [Commented] Phải approve trước khi round 1 bắt đầu — bỏ check để dễ test
//if (firstRound != null && firstRound.StartTime.HasValue && DateTimeOffset.UtcNow >= firstRound.StartTime.Value)
//    throw new BadRequestException("Cannot Approve After Round 1 Has Started");
```

### 8. RejectRegisterTeam
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/reject`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `RejectRegisterTeam`

**Check bị comment:**
```csharp
// [Commented] Check event time window — bỏ check để dễ test
//var ev = await _eventRepository.GetByIdAsync(rt.EventId);
//if (ev != null)
//{
//    if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
//        throw new BadRequestException("Cannot Reject Before Event Starts");
//    if (ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
//        throw new BadRequestException("Cannot Reject After Event Has Ended");
//}
```

### 9. AssignToNextRound
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/assign-next-round`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `AssignToNextRound`

**Check bị comment:**
```csharp
// [Commented] Chỉ được up round khi event chưa kết thúc — bỏ check để dễ test
//var ev = await _eventRepository.GetByIdAsync(rt.EventId);
//if (ev != null && ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
//    throw new BadRequestException("Cannot Assign To Next Round After Event Has Ended");
```

### 10. RevertToPreviousRound
**API:** `POST /api/v1/admin/register-teams/{registerTeamId}/revert-previous-round`
**File:** `Hackathon.Application/Services/Admin/RegisterTeam/Service.cs`
**Hàm:** `RevertToPreviousRound`

**Check bị comment:**
```csharp
// [Commented] Chỉ được down round khi event chưa kết thúc — bỏ check để dễ test
//var ev = await _eventRepository.GetByIdAsync(rt.EventId);
//if (ev != null && ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
//    throw new BadRequestException("Cannot Revert To Previous Round After Event Has Ended");
```

---

## IV. Staff — RegisterTeam

### 11. ApproveRegisterTeam
**API:** `POST /api/v1/staff/register-teams/{registerTeamId}/approve`
**File:** `Hackathon.Application/Services/Staff/RegisterTeam/Service.cs`
**Hàm:** `ApproveRegisterTeam`
> Giống Admin mục 7

### 12. RejectRegisterTeam
**API:** `POST /api/v1/staff/register-teams/{registerTeamId}/reject`
**File:** `Hackathon.Application/Services/Staff/RegisterTeam/Service.cs`
**Hàm:** `RejectRegisterTeam`
> Giống Admin mục 8

### 13. AssignToNextRound
**API:** `POST /api/v1/staff/register-teams/{registerTeamId}/assign-next-round`
**File:** `Hackathon.Application/Services/Staff/RegisterTeam/Service.cs`
**Hàm:** `AssignToNextRound`
> Giống Admin mục 9

### 14. RevertToPreviousRound
**API:** `POST /api/v1/staff/register-teams/{registerTeamId}/revert-previous-round`
**File:** `Hackathon.Application/Services/Staff/RegisterTeam/Service.cs`
**Hàm:** `RevertToPreviousRound`
> Giống Admin mục 10

---

## V. Judge

### 15. GradeSubmission
**API:** `POST /api/v1/judge/submissions/{submissionId}`
**File:** `Hackathon.Application/Services/Judge/Service.cs`
**Hàm:** `GradeSubmission`

**Check bị comment:**
```csharp
// [Commented] Điều kiện chấm: round đã kết thúc (EndTime) ≤ now < Event.EndTime — bỏ check để dễ test
//if (round.EndTime.HasValue && DateTimeOffset.UtcNow < round.EndTime.Value)
//    throw new BadRequestException("Round Has Not Ended Yet. Cannot Grade Before Round End Time.");
//var ev = await _eventRepository.GetByIdAsync(registerTeam.EventId);
//if (ev != null && ev.EndTime.HasValue && DateTimeOffset.UtcNow >= ev.EndTime.Value)
//    throw new BadRequestException("Event Has Ended. Cannot Grade.");
```

### 16. UpdateGradeSubmission
**API:** `PATCH /api/v1/judge/scores/{scoreId}`
**File:** `Hackathon.Application/Services/Judge/Service.cs`
**Hàm:** `UpdateGradeSubmission`

**Check bị comment:**
```csharp
// [Commented] Validate event chưa kết thúc — bỏ check để dễ test
//var registerTeam = score.Submission?.RoundDetail?.RegisterTeam;
//if (registerTeam != null)
//{
//    var ev = await _eventRepository.GetByIdAsync(registerTeam.EventId);
//    if (ev != null && ev.EndTime.HasValue && ev.EndTime.Value <= DateTimeOffset.UtcNow)
//        throw new BadRequestException("Event Has Ended. Cannot Update Score.");
//}
```

### 17. UpdateScoreItem
**API:** `PATCH /api/v1/judge/scores/items/{scoreItemId}`
**File:** `Hackathon.Application/Services/Judge/Service.cs`
**Hàm:** `UpdateScoreItem`

**Check bị comment:**
```csharp
// [Commented] Validate event chưa kết thúc — bỏ check để dễ test
//var registerTeam = scoreItem.ScoreEntity?.Submission?.RoundDetail?.RegisterTeam;
//if (registerTeam != null)
//{
//    var ev = await _eventRepository.GetByIdAsync(registerTeam.EventId);
//    if (ev != null && ev.EndTime.HasValue && ev.EndTime.Value <= DateTimeOffset.UtcNow)
//        throw new BadRequestException("Event Has Ended. Cannot Update Score.");
//}
```

---

## VI. Student

### 18. CreateRegisterTeam
**API:** `POST /api/v1/student/register-events`
**File:** `Hackathon.Application/Services/Student/RegisterTeam/Service.cs`
**Hàm:** `CreateRegisterTeam`

**Check bị comment:**
```csharp
// [Commented] Check registration is within the allowed time window — bỏ check để dễ test
//if (ev.RegisterLimitTime.HasValue && DateTimeOffset.UtcNow >= ev.RegisterLimitTime.Value)
//    throw new BadRequestException("Registration Period Has Ended. Cannot Register At This Time.");
//if (ev.StartTime.HasValue && DateTimeOffset.UtcNow < ev.StartTime.Value)
//    throw new BadRequestException("Registration Has Not Started Yet. Cannot Register Before Event Starts.");
```

**✅ Giữ lại:**
```csharp
if (ev.Status == Domain.Enums.Event.EventStatusEnum.Draft || ev.Status == Domain.Enums.Event.EventStatusEnum.Closed)
    throw new BadRequestException("Cannot Register to a Draft or Closed Event");
```

---

## VII. Tổng kết

| # | API | Commented | Giữ lại |
|---|-----|-----------|---------|
| 1 | Admin POST CreateEvent | EndTime, RegisterLimitTime | — |
| 2 | Admin PATCH UpdateEvent | StartTime, EndTime, RegisterLimitTime | — |
| 3 | Admin POST CreateRound | _(đã comment từ trước)_ | Round time trong event time ✅ |
| 4 | Admin PATCH UpdateRound | _(đã comment từ trước)_ | Round time trong event time ✅ |
| 5 | Admin POST SwapRound | Event start check | — |
| 6 | Admin POST EndRound | IsDisable, start/end checks | — |
| 7 | Admin POST ApproveRegisterTeam | Event time window, Round1 check | Conflict check, Round full |
| 8 | Admin POST RejectRegisterTeam | Event time window | — |
| 9 | Admin POST AssignToNextRound | Event ended check | Last round, Round full |
| 10 | Admin POST RevertToPreviousRound | Event ended check | Has submission check |
| 11 | Staff POST ApproveRegisterTeam | Giống Admin mục 7 | Giống Admin |
| 12 | Staff POST RejectRegisterTeam | Giống Admin mục 8 | — |
| 13 | Staff POST AssignToNextRound | Giống Admin mục 9 | Giống Admin |
| 14 | Staff POST RevertToPreviousRound | Giống Admin mục 10 | Giống Admin |
| 15 | Judge POST GradeSubmission | Round.End, Event.End | Judge track assignment |
| 16 | Judge PATCH UpdateGradeSubmission | Event ended | — |
| 17 | Judge PATCH UpdateScoreItem | Event ended | — |
| 18 | Student POST CreateRegisterTeam | RegisterLimit, StartTime | Draft/Closed check ✅ |

## Cách bật lại

1. Mở file tương ứng
2. Search `[Commented]` — mỗi block comment đều có tag này
3. Xóa `//` ở mỗi dòng trong block đó
4. Build lại: `dotnet build`
