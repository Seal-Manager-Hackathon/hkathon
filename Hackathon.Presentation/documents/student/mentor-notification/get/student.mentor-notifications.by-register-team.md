# GET /api/v1/student/register-teams/{registerTeamId}/mentor-notifications

> Student xem các thông báo mà mentor đã gửi cho track mà team của mình đang tham gia.

**Controller:** [StudentNotificationController.cs](Controllers/Student/StudentNotificationController.cs)

## Nghiệp vụ

- Truyền vào `registerTeamId` — hệ thống tự xác định track của register team đó.
- Tìm tất cả mentor được phân công vào track đó (qua bảng AssignTracks).
- Lấy tất cả thông báo mà các mentor đó đã gửi trong track.
- Chỉ hiển thị thông báo của track mà team đang tham gia.
- Phân trang, sắp xếp theo thời gian tạo giảm dần (mới nhất trước).
- Chỉ register team có status **Approved** mới xem được mentor notifications.
- Người gọi phải là **thành viên** của team đó.

## Phân quyền
- ✅ Student — phải là member của team

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "title": "Hỗ trợ tuần này",
        "description": "Các team cần hoàn thành phần database...",
        "mentorFirstName": "Nguyễn",
        "mentorLastName": "Văn B",
        "mentorEmail": "mentor@fpt.edu.vn",
        "mentorAvatarUrl": "https://res.cloudinary.com/...",
        "trackTitle": "Trí tuệ nhân tạo",
        "createdAt": "2026-07-14T10:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc..."
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| mentorFirstName | Tên mentor đã gửi thông báo |
| mentorLastName | Họ mentor |
| mentorAvatarUrl | Avatar của mentor |
| trackTitle | Tên track mà team đang tham gia |
| title | Tiêu đề thông báo |
| description | Nội dung thông báo |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Only Approved Register Team Can View Mentor Notifications | Register team chưa được duyệt |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You Are Not a Member of This Team | User ko phải member của team |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
