# POST /api/v1/admin/notifications

> Admin tạo thông báo. Hành vi khác nhau tuỳ theo TargetType.

## Nghiệp vụ
- **System**: Gửi cho tất cả users — bỏ qua UserId, TeamId
- **Team**: Gửi cho tất cả thành viên trong team — cần `teamId`
- **Personal**: Gửi cho 1 user — cần `userId`. Nếu có cả `teamId` thì phải check user đó có trong team không

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "Thông báo mới",
  "description": "Nội dung thông báo...",
  "targetType": "System",
  "userId": null,
  "teamId": null
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| title | ✅ | |
| description | ✅ | |
| targetType | ✅ | ⚠️ Enum: Personal, Team, System |
| userId | ❌* | Bắt buộc nếu targetType = Personal |
| teamId | ❌* | Bắt buộc nếu targetType = Team |

### Ví dụ
```json
// System — gửi cho tất cả
{ "title": "Hệ thống bảo trì", "description": "...", "targetType": "System" }

// Team — tạo 1 thông báo cho team (không gửi từng user)
{ "title": "Nhắc hạn nộp", "description": "...", "targetType": "Team", "teamId": "guid" }

// Personal — gửi cho 1 user trong team
{ "title": "Chúc mừng", "description": "...", "targetType": "Personal", "userId": "guid", "teamId": "guid" }
```

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid TargetType. Must be: Personal, Team, System | TargetType sai | Báo "Loại không hợp lệ" |
| 400 | TeamId Is Required When TargetType Is Team | TargetType=Team thiếu teamId | Yêu cầu nhập Team |
| 400 | UserId Is Required When TargetType Is Personal | TargetType=Personal thiếu userId | Yêu cầu chọn User |
| 400 | User Is Not In The Specified Team | UserId + TeamId nhưng user ko trong team | Báo "User không trong team" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Team Not Found | teamId không tồn tại | Báo "Team không tồn tại" |
| 404 | User Not Found | userId không tồn tại | Báo "User không tồn tại" |
