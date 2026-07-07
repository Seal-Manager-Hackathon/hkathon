# GET api/v1/admin/events/{eventId}/setup-check

Kiểm tra event đã setup đầy đủ để publish chưa. Được gọi tự động bởi `PATCH events/{eventId}` khi chuyển status Draft → Published.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

## Response (200)

```json
{
  "data": {
    "isComplete": false,
    "missingFields": ["Description", "LimitTeam", "MinMember", "MaxMember", "Round"]
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Error

- `401` — Unauthorized
- `404` — Event Not Found

## Logic

Kiểm tra các field sau của event:
1. **Name** — không null/empty
2. **StartTime** — có và > hiện tại
3. **EndTime** — có và > StartTime
4. **Season** — có giá trị
5. **Description** — không null/empty
6. **LimitTeam** — có và > 0
7. **MinMember** — có và > 0
8. **MaxMember** — có và > 0
9. **Round** — có ít nhất 1 round trong event

Nếu tất cả OK → `isComplete: true, missingFields: []`. Thiếu field nào → `isComplete: false` kèm danh sách.

## Usage

Khi gọi `PATCH events/{eventId}` với `status: "Published"`:
- Tự động gọi `IsEventSetupComplete`
- Nếu `isComplete = false` → throw `BadRequestException`
- Nếu `isComplete = true` → cho phép publish

`IsDisable` hoàn toàn độc lập, không ảnh hưởng gì đến status hay setup check.
