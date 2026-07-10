# POST api/v1/admin/rounds/{roundId}/swap

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
| TargetRoundNo | int | Yes | RoundNo của round muốn đổi với (>= 1, ko được trùng với round hiện tại, phải tồn tại trong event) |

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
- `400` — Target Round No Must Be Greater Than 0 / RoundNo Not Found / Cannot Swap Round With Itself / Cannot Swap A Deleted Round

## Logic

1. Kiểm tra current round tồn tại
2. Lấy EventId từ current round
3. Kiểm tra event tồn tại
4. Tìm target round theo EventId + TargetRoundNo
4. Cả 2 round ko được là **deleted** (IsDisable = true)
5. Swap: RoundNo, StartTime, EndTime, StartSubmission, EndSubmission
