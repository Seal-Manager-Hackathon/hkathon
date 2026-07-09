# GET /api/v1/staff/register-teams/{registerTeamId} — Xem chi tiết team

## Mục đích

Staff xem thông tin chi tiết của 1 đội đã đăng ký, bao gồm thông tin event, team, thành viên, track/topic.

## Business Context

- Staff chỉ xem được nếu được phân công vào event của register team này
- Trả về danh sách thành viên (members) kèm role leader

## Endpoint

```
GET /api/v1/staff/register-teams/{registerTeamId}
```

## Response

```json
{
  "data": {
    "id": "guid",
    "description": "Mô tả",
    "rejectionReason": null,
    "status": "Approved",
    "isBanned": false,
    "isDisable": false,
    "roundId": "guid",
    "roundName": "Vòng 1",
    "roundNo": 1,
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-08T00:00:00Z",
    "eventId": "guid",
    "eventName": "Hackathon",
    "eventDescription": "Mô tả event",
    "eventStartDate": "2026-06-01T00:00:00Z",
    "eventEndDate": "2026-07-01T00:00:00Z",
    "teamId": "guid",
    "teamName": "Tên team",
    "teamCanEdit": false,
    "teamIsDisable": false,
    "teamCreatedAt": "2026-06-01T00:00:00Z",
    "trackId": "guid",
    "trackTitle": "AI",
    "topicId": "guid",
    "topicTitle": "Chatbot",
    "members": [
      {
        "userId": "guid",
        "email": "leader@example.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "avatarUrl": null,
        "isLeader": true,
        "status": "Active"
      }
    ]
  }
}
```

## Error Handling

| Status | Meaning |
|--------|---------|
| 401 | Token hết hạn/thiếu |
| 403 | Không có role Staff hoặc không được phân công vào event |
| 404 | Register team không tồn tại |