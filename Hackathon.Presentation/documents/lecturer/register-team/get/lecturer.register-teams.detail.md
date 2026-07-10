# GET /api/v1/lecturer/register-teams/{registerTeamId}

> Lecturer xem chi tiết một register team. Response giống hệt Admin.

## Nghiệp vụ

- Response giống hệt Admin `GET /api/v1/admin/register-teams/{registerTeamId}`.
- Lecturer phải được phân công vào event của register team này.

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer) — phải được assign vào event tương ứng

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| registerTeamId | Guid | ✅ | ID của register team |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "description": "...",
    "rejectionReason": null,
    "status": "Approved",
    "isBanned": false,
    "isDisable": false,
    "roundId": "guid",
    "roundName": "Vòng 1",
    "roundNo": 1,
    "createdAt": "...",
    "updatedAt": "...",
    "eventId": "guid",
    "eventName": "Hackathon",
    "eventDescription": "...",
    "eventStartDate": "...",
    "eventEndDate": "...",
    "teamId": "guid",
    "teamName": "Team 1",
    "teamCanEdit": false,
    "teamIsDisable": false,
    "teamCreatedAt": "...",
    "trackId": "guid",
    "trackTitle": "AI",
    "topicId": "guid",
    "topicTitle": "Xử lý ảnh",
    "members": [
      {
        "userId": "guid",
        "email": "user@example.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": "...",
        "isLeader": true,
        "status": "Approved"
      }
    ]
  },
  "message": "Register Team Detail Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned to This Event | Lecturer chưa được assign |
| 403 | You do not have permission | Không phải Lecturer |
| 404 | Register Team Not Found | ID ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-teams/{registerTeamId})
