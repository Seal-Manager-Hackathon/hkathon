# GET /api/v1/admin/users/{userId}/teams

> Lấy danh sách team mà user đã tham gia hoặc đã rời, phân trang, có lọc.

## Nghiệp vụ
- Trả về các team mà user đã tham gia qua bảng TeamDetails
- `isDisable = true` → user đã rời team đó
- `isDisable = false` → user đang trong team
- Sắp xếp gần nhất trên cùng

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `FTeam` | Search tên team |
| Status | string | ❌ | `Active` | ⚠️ Enum: Active, Inactive — trạng thái của team detail |
| IsDisable | bool | ❌ | `false` | `true` = đã rời team, `false` = đang trong team, null = tất cả |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "teams": [
      {
        "teamDetailId": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "canEdit": true,
        "isDisable": false,
        "isLeader": true,
        "status": "Active",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
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
| 400 | Invalid Status. Must be: Active, Inactive | Status sai | Báo "Trạng thái không hợp lệ" |
| 400 | Page Index / Page Size | Pagination sai | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
