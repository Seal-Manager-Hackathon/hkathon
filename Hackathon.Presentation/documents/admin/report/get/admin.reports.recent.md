# GET /api/v1/admin/reports/recent

> Lấy 10 reports được tạo gần nhất.

## Nghiệp vụ
- Sắp xếp theo CreatedAt giảm dần, lấy 10 reports mới nhất

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": {
    "reports": [
      {
        "id": "guid",
        "title": "Báo cáo vi phạm",
        "description": "Chi tiết báo cáo",
        "status": "Pending",
        "typeReport": "Bug",
        "createdAt": "2026-07-07T12:00:00Z",
        "userId": "guid",
        "userEmail": "user@example.com",
        "userFirstName": "Nguyen",
        "userLastName": "Van A"
      }
    ]
  },
  "message": "Recent Reports Fetched Successfully",
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
