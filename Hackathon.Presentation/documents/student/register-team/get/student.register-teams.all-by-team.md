# GET /api/v1/student/teams/{teamId}/register-teams/all

> Student lấy tất cả đơn đăng ký (register team) của team mình, bao gồm tất cả trạng thái: đã duyệt, từ chối, chờ duyệt, bị ban, đã disable.

## Nghiệp vụ

Student muốn xem toàn bộ lịch sử đăng ký của team:
- Không lọc theo status: trả về tất cả (Pending, Approved, Rejected, Banned).
- Có thể thấy lý do từ chối (RejectionReason) nếu đơn bị từ chối.
- Có thể thấy IsBanned nếu đơn bị ban.
- Có thể thấy IsDisable nếu đơn bị disable.
- Có phân trang.
- Sắp xếp theo CreatedAt giảm dần (mới nhất trước).

> **Ref:** Khác với `GET teams/{teamId}/register-teams` (chỉ lấy Approved), API này lấy **tất cả**.

## Phân quyền
- ✅ Student (RoleEnum = Student)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "teamId": "guid",
        "teamName": "Team ABC",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "trackId": "guid",
        "trackName": "Tri tue nhan tao",
        "topicId": "guid",
        "topicName": "AI trong Y tế",
        "description": "Mô tả đội",
        "rejectionReason": "Lý do từ chối (nếu có)",
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vong 1",
        "roundNo": 1,
        "createdAt": "2026-07-11T10:00:00Z",
        "updatedAt": "2026-07-11T10:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| registerTeams[].status | Trạng thái đơn: Pending / Approved / Rejected / Banned |
| registerTeams[].rejectionReason | Lý do từ chối (null nếu không bị từ chối) |
| registerTeams[].isBanned | Cờ đánh dấu đơn bị cấm |
| registerTeams[].isDisable | Cờ đánh dấu đơn bị disable |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
