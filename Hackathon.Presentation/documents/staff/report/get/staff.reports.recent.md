# GET /api/v1/staff/reports/recent

> Staff lấy 10 báo cáo gần nhất.

## Nghiệp vụ

Lấy nhanh các báo cáo mới nhất (10 bản ghi) không phân trang. Dùng cho màn hình dashboard.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": {
    "reports": [
      {
        "id": "guid",
        "userId": "guid",
        "userEmail": "user@example.com",
        "userFirstName": "John",
        "userLastName": "Doe",
        "title": "Report Title",
        "description": "Nội dung báo cáo",
        "status": "Pending",
        "typeReport": "Spam",
        "createdAt": "2026-07-01T00:00:00Z"
      }
    ]
  },
  "message": "Recent Reports Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/report/get/admin.reports.recent.md)
