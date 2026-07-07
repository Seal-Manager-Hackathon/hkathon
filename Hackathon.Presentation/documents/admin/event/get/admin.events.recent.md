# GET /api/v1/admin/events/recent

> Lấy 5 events được tạo gần nhất.

## Nghiệp vụ
- Sắp xếp theo CreatedAt giảm dần, lấy 5 events mới nhất

## Phân quyền
- ✅ Admin

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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
