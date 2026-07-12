# GET /api/v1/student/events/{eventId}/teams/{teamId}/register-teams

> Student lấy danh sách register team của team mình gửi tới 1 event cụ thể, kèm trạng thái phản hồi.

**Controller:** [StudentTrackTopicRegisterTeamController.cs](Controllers/Student/StudentTrackTopicRegisterTeamController.cs)

## Nghiệp vụ

Student muốn xem team mình đã đăng ký event nào chưa, trạng thái ra sao:
- Lọc theo `eventId` và `teamId`.
- Có thể lọc theo status: `Pending`, `Approved`, `Rejected`, `Banned`. Ko truyền = lấy hết.
- Sắp xếp theo `CreatedAt` giảm dần (mới nhất trước).
- Có phân trang.

## Phân quyền
- ✅ Student

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |
| teamId | Guid | ID của team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| status | string | No | - | ⚠️ Lọc theo trạng thái: Pending, Approved, Rejected, Banned |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số item mỗi trang |

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
        "rejectionReason": null,
        "status": "Pending",
        "isBanned": false,
        "isDisable": false,
        "roundId": null,
        "roundName": null,
        "roundNo": null,
        "createdAt": "2026-07-13T10:00:00Z",
        "updatedAt": "2026-07-13T10:00:00Z"
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

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status sai |
| 400 | Page Index / Page Size | Pagination sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
