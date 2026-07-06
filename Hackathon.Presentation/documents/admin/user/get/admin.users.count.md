# GET /api/v1/admin/users/count

> Đếm số lượng users. Có thể lọc theo role, không lọc thì lấy tất cả.

## Nghiệp vụ
- Không truyền `Role` → đếm tất cả users
- Có truyền `Role` → đếm users theo role đó

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Role | string | ❌ | Staff | ⚠️ Enum: Admin, Staff, Student, Lecturer |

### Ví dụ
```
GET /api/v1/admin/users/count
GET /api/v1/admin/users/count?Role=Staff
```

## Response (200)
```json
{
  "data": { "total": 42 },
  "message": "User Count Fetched Successfully",
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
| 400 | Invalid Role. Must be: Admin, Staff, Student, Lecturer | Role sai | Báo "Vai trò không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | User Not Found | User không tồn tại | Báo lỗi |
