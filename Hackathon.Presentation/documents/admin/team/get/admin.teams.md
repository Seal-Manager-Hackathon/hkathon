# GET /api/v1/admin/teams

> Lấy danh sách teams phân trang, có lọc theo keyword, canEdit, thời gian tạo.

## Nghiệp vụ
- Keyword search theo tên team (contains)
- Lọc CanEdit (true/false)
- Lọc FromDate / ToDate theo CreatedAt
- Sắp xếp gần nhất trên cùng

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `FTeam` | Search tên team |
| CanEdit | bool | ❌ | `true` | |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

### Ví dụ
```
GET /api/v1/admin/teams                              → Tất cả
GET /api/v1/admin/teams?Keyword=FTeam                → Search tên
GET /api/v1/admin/teams?CanEdit=true                 → Lọc canEdit
GET /api/v1/admin/teams?FromDate=...&ToDate=...      → Lọc thời gian
```

## Response (200)
```json
{
  "data": {
    "teams": [
      {
        "id": "guid",
        "name": "FTeam",
        "canEdit": true,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Teams Fetched Successfully",
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
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 | Fix pagination |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
