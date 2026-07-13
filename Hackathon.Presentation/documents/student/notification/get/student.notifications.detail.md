# GET /api/v1/student/notifications/{notificationId}

> Student lấy thông tin chi tiết của 1 notification (Personal/Team/System) mà họ có quyền xem.

## Nghiệp vụ

API này cho phép student đã đăng nhập xem chi tiết thông báo từ bảng Notifications (khác MentorNotifications).

- **System notification:** Tất cả student đều xem được.
- **Personal notification (UserId trùng):** Chỉ student sở hữu mới xem được.
- **Team notification (TeamId):** Chỉ thành viên đang hoạt động trong team đó mới xem được.
- Nếu không thuộc các trường hợp trên → 403 Forbidden.
- Chỉ trả về notification có IsDisable = false.

> **Ref:** [Base notification detail API](/api/v1/notifications/{notificationId})

## Phân quyền
- ✅ Student (đã đăng nhập, token hợp lệ)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| notificationId | Guid | ID của notification |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": "guid/null",
    "teamId": "guid/null",
    "title": "Thông báo khẩn cấp",
    "status": "Unread",
    "description": "Nội dung chi tiết thông báo...",
    "targetType": "System",
    "createdAt": "2026-07-14T12:00:00Z",
    "updatedAt": "2026-07-14T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| id | ID của notification |
| userId | User nhận (đối với Personal notification), null nếu System/Team |
| teamId | Team nhận (đối với Team notification), null nếu Personal/System |
| title | Tiêu đề notification |
| status | Trạng thái: Unread / Read |
| description | Nội dung chi tiết |
| targetType | Đối tượng đích: System / Personal / Team |
| createdAt | Thời gian tạo |
| updatedAt | Thời gian cập nhật gần nhất |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | You Do Not Have Access to This Notification | Xem notification của người/team khác |
| 404 | Notification Not Found | ID không tồn tại hoặc đã bị disable |

## Luồng xử lý

1. Controller nhận `notificationId` từ route, gọi Service.
2. Service lấy `UserId` từ JWT claims.
3. Gọi `INotificationRepository.GetDetailByIdAsync` (có Include User, Team).
4. Kiểm tra notification tồn tại và `IsDisable == false`, nếu không → 404.
5. Kiểm tra quyền truy cập:
   - `UserId == currentUserId` (Personal) → OK
   - `TargetType == System` → OK
   - `TargetType == Team` và `TeamId` thuộc danh sách team đang hoạt động của user → OK
   - Otherwise → 403.
6. Map sang `StudentNotificationDetailResponse` và trả về.
