# GET /api/v1/lecturer/teams/{teamId}/events

> Lecturer lấy danh sách event mà team đã tham gia.

**Controller:** [LecturerTeamController.cs](Controllers/Lecturer/LecturerTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/teams/{teamId}/events`

- Giống hệt Admin `GET /api/v1/admin/teams/{teamId}/events`, khác auth là Lecturer.
- Lấy các event mà team đã được approve tham gia (RegisterTeam Status=Approved, IsDisable=false).
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| teamId | Guid | ID của team |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số bản ghi mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "eventId": "guid",
        "eventName": "Hackathon AI 2026",
        "status": "Published",
        "createdAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/{teamId}/events)
