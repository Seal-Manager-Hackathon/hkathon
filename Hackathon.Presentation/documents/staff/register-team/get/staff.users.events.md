# GET /api/v1/staff/users/{userId}/events — DS event của 1 user

## Mục đích

Staff xem danh sách các event mà 1 user đã tham gia (với tư cách thành viên team).

## Endpoint

```
GET /api/v1/staff/users/{userId}/events
```

## Controller

`StaffRegisterTeamController.GetUserEvents()` → `IRegisterTeamService.GetUserEvents()`.

## Response

```json
{
  "data": {
    "events": [
      {
        "registerTeamId": "guid",
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-08T00:00:00Z",
        "eventId": "guid",
        "eventName": "Hackathon",
        "eventDescription": "...",
        "eventStartTime": "2026-06-01T00:00:00Z",
        "eventEndTime": "2026-07-01T00:00:00Z",
        "eventStatus": "Ongoing",
        "teamId": "guid",
        "teamName": "Tên team",
        "trackId": "guid",
        "trackTitle": "AI",
        "topicId": "guid",
        "topicTitle": "Chatbot"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```