# POST /api/v1/staff/notifications

> Staff tạo thông báo mới.

## Nghiệp vụ

Tạo thông báo với các loại: System (không gán ai), Team (gán cho team), Personal (gán cho user).

## Phân quyền
- ✅ Staff

## Request Body
```json
{
  "title": "Thông báo mới",
  "description": "Nội dung thông báo",
  "targetType": "System",
  "userId": null,
  "teamId": null
}
```

### Field ý nghĩa
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| title | string | Yes | Tiêu đề thông báo |
| description | string | Yes | Nội dung thông báo |
| targetType | string | Yes | Personal / Team / System |
| userId | guid | No | Bắt buộc nếu targetType=Personal |
| teamId | guid | No | Bắt buộc nếu targetType=Team |

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "status": 201
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid TargetType | TargetType không hợp lệ |
| 400 | TeamId Is Required When TargetType Is Team | Thiếu TeamId |
| 400 | UserId Is Required When TargetType Is Personal | Thiếu UserId |
| 404 | Team Not Found | TeamId không tồn tại |
| 404 | User Not Found | UserId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notification/post/admin.notifications.md)
