# GET /api/v1/admin/notifications

> Lấy danh sách thông báo phân trang, có lọc theo title, targetType, thời gian tạo.

## Nghiệp vụ
- Không truyền filter gì → lấy tất cả, sắp xếp gần nhất trên cùng
- Title search dạng contains (không phân biệt hoa/thường)
- TargetType filter enum: Personal, Team, System
- FromDate / ToDate lọc theo CreatedAt

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Title | string | ❌ | `Thông báo` | Search contains |
| TargetType | string | ❌ | `Personal` | ⚠️ Enum: Personal, Team, System |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | Lọc từ ngày |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | Lọc đến ngày |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

### Ví dụ
```
GET /api/v1/admin/notifications                                     → Tất cả
GET /api/v1/admin/notifications?Title=hackathon                     → Search title
GET /api/v1/admin/notifications?TargetType=System                   → Lọc target type
GET /api/v1/admin/notifications?FromDate=2026-07-01T00:00:00Z&ToDate=2026-07-07T23:59:59Z  → Lọc thời gian
GET /api/v1/admin/notifications?Title=thi&TargetType=Personal&PageSize=5  → Kết hợp nhiều filter
```

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "userId": "guid",
        "teamId": null,
        "title": "Thông báo mới",
        "status": "Unread",
        "description": "Nội dung thông báo...",
        "targetType": "Personal",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Notifications Fetched Successfully",
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
| 400 | Invalid TargetType. Must be: Personal, Team, System | TargetType sai | Báo "Loại thông báo không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
