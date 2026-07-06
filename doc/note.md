# Exception Message Convention

## Phân biệt messageCode và message

```
AppException(title, statusCode, messageCode, message)
```

| Field | Ý nghĩa | Ví dụ |
|-------|---------|-------|
| `messageCode` | Trạng thái ngắn gọn — generic, tái sử dụng | `FIELD_REQUIRED`, `NUMBER_MUST_BE_POSITIVE` |
| `message` | Lý do chi tiết — mô tả cụ thể lỗi gì | `EVENT_NAME_IS_REQUIRED`, `LIMIT_TEAM_MUST_BE_GREATER_THAN_1` |

## Format: UPPER_SNAKE_CASE

- Full English, all caps, words separated by underscore
- Cả `messageCode` và `message` đều theo format này

## Common Patterns

| Pattern | messageCode | message |
|---------|-------------|---------|
| Required (empty) | `FIELD_REQUIRED` | `{FIELD}_IS_REQUIRED` |
| Invalid format | `FIELD_INVALID` | `{FIELD}_IS_INVALID` |
| Not found | `FIELD_NOT_FOUND` | `{FIELD}_NOT_FOUND` |
| Already exists | `FIELD_ALREADY_EXISTS` | `{FIELD}_ALREADY_EXISTS` |
| Already open | `FIELD_ALREADY_OPEN` | `{FIELD}_ALREADY_OPEN` |
| Full / capacity reached | `FIELD_FULL` | `{FIELD}_IS_FULL_{CAPACITY}` |
| Must be positive (>0) | `NUMBER_MUST_BE_POSITIVE` | `{FIELD}_MUST_BE_POSITIVE` |
| Must not be negative (>=0) | `NUMBER_MUST_NOT_BE_NEGATIVE` | `{FIELD}_MUST_NOT_BE_NEGATIVE` |
| Exceeds max | `NUMBER_EXCEED_MAX` | `{FIELD}_EXCEEDS_MAX_{MAX}` |
| Below min | `NUMBER_BELOW_MIN` | `{FIELD}_BELOW_MIN_{MIN}` |
| Too long | `FIELD_TOO_LONG` | `{FIELD}_EXCEEDS_MAX_LENGTH_{MAX}` |
| Too short | `FIELD_TOO_SHORT` | `{FIELD}_BELOW_MIN_LENGTH_{MIN}` |

## Examples

```csharp
// messageCode = "FIELD_REQUIRED", message = "EVENT_NAME_IS_REQUIRED"
public class EventNameRequiredException : BadRequestException
{
    public EventNameRequiredException()
        : base(ErrorMessage.Events.NameRequired, "EVENT_NAME_IS_REQUIRED") { }
}

// messageCode = "NUMBER_MUST_BE_POSITIVE", message = "LIMIT_TEAM_MUST_BE_GREATER_THAN_1"
public class EventLimitTeamInvalidException : BadRequestException
{
    public EventLimitTeamInvalidException()
        : base(ErrorMessage.BaseEntity.NumberMustBePositive, "LIMIT_TEAM_MUST_BE_GREATER_THAN_1") { }
}
```

## Rules
1. Full English only — no Vietnamese
2. All uppercase
3. Words separated by underscore (`_`)
4. Field names in UPPER_SNAKE_CASE
5. Dynamic values appended at the end: `_{VALUE}`
