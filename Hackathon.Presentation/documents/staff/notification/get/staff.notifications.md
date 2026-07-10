# GET /api/v1/staff/notifications

> Staff lấy danh sách thông báo với filter và phân trang.

## Nghiệp vụ

Staff xem toàn bộ thông báo trong hệ thống (giống Admin). API này giúp staff quản lý các thông báo gửi đến user, team hoặc toàn hệ thống.

- Hỗ trợ lọc theo title, target type, khoảng thời gian, trạng thái disable.
- Hỗ trợ tìm kiếm theo từ khóa (title).
- Phân trang mặc định page 1, pageSize 10.
- Sắp xếp theo CreatedAt DESC.

## Phân quyền
- ✅ Staff

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| title | string | No | - | Lọc theo tiêu đề |
| targetType | string | No | - | Personal / Team / System |
| fromDate | datetime | No | - | Lọc từ ngày |
| toDate | datetime | No | - | Lọc đến ngày |
| isDisable | bool | No | - | Lọc theo trạng thái disable |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số bản ghi mỗi trang |

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "userId": "guid",
        "teamId": "guid",
        "title": "Notification Title",
        "status": "Unread",
        "description": "Chi tiết thông báo",
        "targetType": "System",
        "isDisable": false,
        "createdAt": "2026-07-01T00:00:00Z",
        "updatedAt": "2026-07-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Notifications Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Staff |
