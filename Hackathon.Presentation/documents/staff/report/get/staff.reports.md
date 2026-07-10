# GET /api/v1/staff/reports

> Staff lấy danh sách báo cáo với filter và phân trang.

## Nghiệp vụ

Staff xem toàn bộ báo cáo trong hệ thống (giống Admin). Dùng để quản lý các báo cáo từ người dùng.

- Hỗ trợ lọc theo từ khóa (keyword tìm trong title) và status.
- Phân trang mặc định page 1, pageSize 10.
- Sắp xếp theo CreatedAt DESC.

## Phân quyền
- ✅ Staff

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm kiếm theo tiêu đề |
| status | string | No | - | Pending / Reject / Resolved |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số bản ghi mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
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
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status không hợp lệ |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/report/get/admin.reports.list.md)
