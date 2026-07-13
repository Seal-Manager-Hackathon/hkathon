# GET /api/v1/student/mentor-notifications/{mentorNotificationId}

> Student xem chi tiết 1 thông báo của mentor — gồm thông tin mentor, event, track.

**Controller:** [StudentNotificationController.cs](Controllers/Student/StudentNotificationController.cs)

## Nghiệp vụ

- Trả về chi tiết thông báo mentor: tiêu đề, nội dung, thông tin mentor (tên, email, avatar, role), event, track.
- Chỉ user nào là **thành viên của team có track trùng với track của notification** mới xem được.
- Team phải có register team **Approved** trong event đó.

## Phân quyền
- ✅ Student — phải là member của team thuộc track được mentor phụ trách

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| mentorNotificationId | Guid | ID của mentor notification |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "title": "Hỗ trợ tuần này",
    "description": "Các team cần hoàn thành phần database...",
    "mentorUserId": "guid",
    "mentorFirstName": "Nguyễn",
    "mentorLastName": "Văn B",
    "mentorEmail": "mentor@fpt.edu.vn",
    "mentorAvatarUrl": "https://res.cloudinary.com/...",
    "mentorRole": "Mentor",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "trackId": "guid",
    "trackTitle": "Trí tuệ nhân tạo",
    "createdAt": "2026-07-14T10:00:00Z",
    "updatedAt": "2026-07-14T10:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc..."
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| mentorUserId | ID của mentor |
| mentorRole | Vai trò của mentor trong event (Judge/Mentor/Staff) |
| eventId | ID của event |
| eventName | Tên event |
| trackId | ID của track |
| trackTitle | Tên track |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You Do Not Have Access to This Mentor Notification | User ko thuộc team nào trong track này |
| 404 | Mentor Notification Not Found | mentorNotificationId ko tồn tại |
