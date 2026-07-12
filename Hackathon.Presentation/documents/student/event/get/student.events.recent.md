# GET /api/v1/student/events/recent

> Student lấy danh sách 10 event mới nhất (Published/Closed, ko disable).

**Controller:** [StudentEventController.cs](Controllers/Student/StudentEventController.cs)

## Nghiệp vụ
- Lấy 10 event mới nhất (sắp xếp theo CreatedAt giảm dần).
- **Tự động filter** chỉ lấy event có `Status != Draft` và `IsDisable = false`.
- Không hỗ trợ phân trang — mặc định lấy 10 cái mới nhất.

## Phân quyền
- ✅ Student

## Request
Không query params, không body.

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "Event description",
        "status": "Published",
        "startTime": "2026-07-01T00:00:00Z",
        "endTime": "2026-07-10T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-10T00:00:00Z",
        "updatedAt": "2026-07-10T00:00:00Z"
      }
    ]
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/recent)
