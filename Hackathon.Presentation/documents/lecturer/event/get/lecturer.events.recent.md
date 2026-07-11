# GET /api/v1/lecturer/events/recent

> Lecturer lấy 10 events được phân công gần nhất.

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/recent`

- **Khác Admin:** Chỉ lấy event mà Lecturer được phân công (AssignEvents với EventRole = Judge/Mentor).
- Response format giống Admin.
- Sắp xếp theo CreatedAt giảm dần, lấy 10 events mới nhất.

## Phân quyền
- ✅ Lecturer

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "Cuộc thi lập trình",
        "status": "Published",
        "startTime": "2026-08-01T00:00:00Z",
        "endTime": "2026-08-03T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ]
  },
  "message": "Recent Events Fetched Successfully",
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/recent)
