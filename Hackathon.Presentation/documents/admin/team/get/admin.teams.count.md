# GET /api/v1/admin/teams/count

> Đếm số lượng teams. Có thể lọc theo trạng thái IsDisable.

## Nghiệp vụ
- Không truyền IsDisable → đếm tất cả
- IsDisable=true → team bị disable
- IsDisable=false → team active

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| IsDisable | bool | ❌ | true | true/false. Không truyền = lấy tất cả |

### Ví dụ
```
GET /api/v1/admin/teams/count
GET /api/v1/admin/teams/count?IsDisable=true
```

## Response (200)
```json
{
  "data": { "total": 15 },
  "message": "Team Count Fetched Successfully",
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
| 400 | Invalid Request Data | IsDisable không phải true/false | Báo "Dữ liệu không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
