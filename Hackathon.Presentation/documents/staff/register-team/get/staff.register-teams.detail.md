# GET /api/v1/staff/register-teams/{registerTeamId}

> Xem chi tiết thông tin một đội đã đăng ký, bao gồm thông tin event, team, track/topic và danh sách thành viên.

## Nghiệp vụ
- Staff chỉ xem được nếu được phân công vào event chứa register team này
- Trả về đầy đủ thông tin: event (tên, mô tả, thời gian), team (tên, trạng thái chỉnh sửa), track/topic, danh sách thành viên kèm role leader

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team |

## Response (200)

```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "description": "Mô tả dự án",
    "rejectionReason": null,
    "status": "Approved",
    "isBanned": false,
    "isDisable": false,
    "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "roundName": "Vòng 1",
    "roundNo": 1,
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-08T00:00:00Z",
    "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventName": "Hackathon 2026",
    "eventDescription": "Sự kiện hackathon thường niên",
    "eventStartDate": "2026-06-01T00:00:00Z",
    "eventEndDate": "2026-07-01T00:00:00Z",
    "teamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "teamName": "Team A",
    "teamCanEdit": false,
    "teamIsDisable": false,
    "teamCreatedAt": "2026-06-01T00:00:00Z",
    "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "trackTitle": "AI",
    "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "topicTitle": "Chatbot",
    "members": [
      {
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "leader@example.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "avatarUrl": null,
        "isLeader": true,
        "status": "Active"
      }
    ]
  },
  "message": "Lấy thông tin register team thành công",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-0b4e4e4b7b8c4d4f8f9a0b1c2d3e4f5a",
  "timestampUtc": "2026-07-09T10:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | `Unauthorized` | Token hết hạn hoặc thiếu | Redirect sang trang login |
| 403 | `Forbidden` | Không có role Staff hoặc không được phân công vào event | Hiển thị thông báo không có quyền |
| 404 | `Register team not found` | registerTeamId không tồn tại | Hiển thị thông báo không tìm thấy |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-team/get/admin.register-teams.detail.md)
