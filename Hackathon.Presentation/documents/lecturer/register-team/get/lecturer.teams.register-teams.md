# GET /api/v1/lecturer/teams/{teamId}/register-teams

> Lecturer xem danh sách register teams của một team. Response giống hệt Admin.

## Nghiệp vụ

- Response giống hệt Admin `GET /api/v1/admin/teams/{teamId}/register-teams`.
- Không check assignment (vì chỉ xem thông tin team, ko cần event).

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| teamId | Guid | ✅ | ID của team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Status | string | No | - | Lọc: Pending, Approved, Rejected, Banned |
| IsDisable | bool | No | - | Lọc disable |
| PageIndex | int | No | 1 | Trang |
| PageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team 1",
        "eventId": "guid",
        "eventName": "Hackathon",
        "trackId": "guid",
        "trackName": "AI",
        "topicId": "guid",
        "topicName": "Xử lý ảnh",
        "description": "...",
        "rejectionReason": null,
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "createdAt": "...",
        "updatedAt": "..."
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Register Teams Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status sai |
| 400 | PageIndex/PageSize invalid | Pagination sai |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId}/register-teams)
