# POST /api/v1/admin/events/{eventId}/awards/{awardId}/swap

> Admin đổi thứ hạng (LevelAward) giữa 2 award trong cùng event.

## Nghiệp vụ
- Chỉ số targetLevel phải >= 1
- Không được swap với chính nó
- Target level phải tồn tại trong event
- Chỉ swap level, không ảnh hưởng các field khác

## Phân quyền
- ✅ Admin

## Request
Body (JSON):
```json
{
  "targetLevel": 2
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| targetLevel | ✅ | >= 1 |

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Target Level Must Be Greater Than 0 | targetLevel < 1 |
| 400 | Cannot Swap A Deleted Award | award đã bị disable |
| 400 | Cannot Swap Award With Itself | targetLevel == level hiện tại |
| 400 | Target Level Not Found In This Event | ko có award nào có level đó |
| 404 | Resource Not Found | awardId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
