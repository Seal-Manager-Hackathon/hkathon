# GET /api/v1/staff/users/count

> Staff đếm số lượng users. Có thể lọc theo role.

**Controller:** [StaffUserController.cs](Controllers/Staff/StaffUserController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/staff/users/count`

- Giống hệt Admin `GET /api/v1/admin/users/count`, khác auth là Staff.

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Role | string | ❌ | Staff | ⚠️ Enum: Admin, Staff, Student, Lecturer |

## Response (200)
```json
{
  "data": { "total": 42 },
  "message": "User Count Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Role. Must be: Admin, Staff, Student, Lecturer | Role sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/users/count)
