# POST /api/v1/admin/awards/{awardId}/swap

> Admin đổi thứ hạng (LevelAward) giữa 2 award trong cùng event.

## Nghiệp vụ
- Chỉ số targetLevel phải >= 1
- Không được swap với chính nó
- Target level phải tồn tại trong event
- Chỉ swap level, không ảnh hưởng các field khác
- Không thể swap với giải đã bị xóa (IsDisable = true hoặc LevelAward = 0)
- Giải đích cũng phải còn active (chưa bị xóa), nếu giải đích đã bị xóa → báo lỗi

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
| 400 | Cannot Swap A Deleted Award | Giải hiện tại đã bị xóa | Chỉ swap giải đang active |
| 400 | Cannot Swap Award With Itself | targetLevel == level hiện tại |
| 400 | Target Level Not Found In This Event | Giải đích không tồn tại hoặc đã bị xóa |
| 404 | Resource Not Found | awardId ko tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
