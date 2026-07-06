# Business Rules & Exception Registry

> Mỗi business rule PHẢI được ghi vào đây khi thêm vào code.
> Format: Entity → Method → Exception → Layer (Domain/Application/Infrastructure)

## Quy tắc

1. **Domain layer** — exception được throw từ entity methods (`Create()`, `Update()`, etc.)
   - Nếu là lỗi chung (required, not negative,...) → throw **base exception** từ `Common/BaseException.cs`
   - Nếu là lỗi đặc thù entity → throw **entity-specific exception** từ `Entities/Exception/{Entity}Exception.cs`

2. **Application layer** — exception được throw từ service layer
   - Dùng `NotFoundException`, `BadRequestException`, etc. từ `Domain/Exceptions/CommonExceptions.cs`

---

## Events

| Phương thức | Business Rule | Exception | Layer |
|-------------|---------------|-----------|-------|
| `Create` | Name không được để trống | `FieldRequiredException("EVENT_NAME")` | Domain |
| `Create` | LimitTeam không được âm | `NumberMustNotBeNegativeException("LIMIT_TEAM")` | Domain |
| `Create` | MinMember không được âm | `NumberMustNotBeNegativeException("MIN_MEMBER")` | Domain |
| `Create` | MaxMember không được âm | `NumberMustNotBeNegativeException("MAX_MEMBER")` | Domain |
| `Create` | LimitTeam phải > 1 | `EventLimitTeamInvalidException` | Domain |
| `Create` | MinMember phải > 1 | `EventMinMemberInvalidException` | Domain |
| `Create` | MaxMember phải > MinMember | `EventMaxMemberInvalidException` | Domain |
| `Create` | StartTime phải > now | `EventStartTimeInvalidException` | Domain |
| `Create` | EndTime phải > StartTime | `EventEndTimeInvalidException` | Domain |
| `Create` | RegisterLimitTime phải giữa StartTime và EndTime | `EventRegisterLimitTimeInvalidException` | Domain |
| `Create` | Mặc định NumberRound = 0, Status = Draft | — | Domain |
| `Update` | Check lại tất cả rules như Create (mỗi cổng vào là 1 lần check) | Như Create | Domain |
| `Publish` | Không thể publish nếu đã Published | `EventAlreadyPublishedException` | Domain |
| `Publish` | Không thể publish nếu đã Closed | `EventAlreadyClosedException` | Domain |
| `Close` | Không thể close nếu đã Closed | `EventAlreadyClosedException` | Domain |

---

## Users

| Phương thức | Business Rule | Exception | Layer |
|-------------|---------------|-----------|-------|

*(đang chờ)*

---

## Teams

| Phương thức | Business Rule | Exception | Layer |
|-------------|---------------|-----------|-------|

*(đang chờ)*

---

## Rounds

| Phương thức | Business Rule | Exception | Layer |
|-------------|---------------|-----------|-------|

*(đang chờ)*
