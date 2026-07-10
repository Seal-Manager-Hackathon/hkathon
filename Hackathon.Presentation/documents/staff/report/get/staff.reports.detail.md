# GET /api/v1/staff/reports/{reportId}

> Staff xem chi tiết báo cáo.

## Nghiệp vụ

Lấy thông tin chi tiết của một report theo ID.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "userId": "guid",
    "userEmail": "user@example.com",
    "userFirstName": "John",
    "userLastName": "Doe",
    "title": "Report Title",
    "description": "Nội dung báo cáo",
    "status": "Pending",
    "reason": null,
    "typeReport": "Spam",
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | ID không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |
