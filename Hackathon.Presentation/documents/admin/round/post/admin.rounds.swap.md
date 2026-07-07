# POST api/v1/admin/events/{eventId}/rounds/{roundId}/swap

Swap thứ tự 2 round trong cùng event.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |
| roundId | Guid | ID của round cần đổi |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| TargetRoundNo | int | Yes | RoundNo của round muốn đổi với |

## Response

```json
{
  "data": null,
  "message": "Operation Successful",
  "traceId": "..."
}
```

## Error

- `401` — Unauthorized
- `404` — Event Not Found / Round Not Found
- `400` — RoundNo Not Found / Cannot Swap Round With Itself / Cannot Swap A Deleted Round

## Logic

1. Kiểm tra event tồn tại
2. Kiểm tra current round tồn tại + thuộc event
3. Tìm target round theo EventId + TargetRoundNo
4. Cả 2 round ko được là **deleted** (IsDisable = true)
5. Swap: RoundNo, StartTime, EndTime, StartSubmission, EndSubmission
