# GET /api/v1/lecturer/users/{userId}/events

> Lecturer lấy danh sách event mà user đã tham gia (register team được approved), phân trang.

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/users/{userId}/events`

- Giống hệt Admin `GET /api/v1/admin/users/{userId}/events`, khác auth là Lecturer.
- Chỉ lấy register team có status `Approved`.
- Có keyword search theo tên event.
- Không có event nào → trả mảng rỗng, không lỗi.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| userId | Guid | ID của user |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tên event |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "registerTeamId": "guid",
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "createdAt": "...",
        "updatedAt": "...",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "eventDescription": "...",
        "eventStartTime": "...",
        "eventEndTime": "...",
        "eventStatus": "Published",
        "teamId": "guid",
        "teamName": "FTeam",
        "trackId": "guid",
        "trackTitle": "Web3",
        "topicId": null,
        "topicTitle": null
      }
    ],
    "totalCount": 5,
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
| 400 | Page Index / Page Size | Pagination sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/{userId}/events)
