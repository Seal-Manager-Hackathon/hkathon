# POST /api/v1/staff/events/{eventId}/assign/users

> Staff phân công Lecturer vào event với vai trò Judge hoặc Mentor.

## Nghiệp vụ
- Staff phải được phân công vào event tương ứng.
- Chỉ có thể assign user có role = `Lecturer`.
- **Không thể** phân công Lecturer đã bị **disable** (`IsDisable = true`) hoặc đã bị **ban** (`BanReason != null`)
- EventRole hợp lệ: `Judge` hoặc `Mentor` — Không thể assign role `Staff`.
- Mỗi Lecturer chỉ được assign vào một event một lần (kiểm tra duplicate).
- Staff assign dựa trên `UserId` của Lecturer.

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của event |

### Body (JSON)
| Field | Type | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| userId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của Lecturer cần assign |
| eventRole | string | Có | Judge | Vai trò trong event: `Judge` hoặc `Mentor` |

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Cannot Assign A Disabled User | Lecturer đang bị disable | Hiển thị thông báo lỗi |
| 400 | Cannot Assign A Banned User | Lecturer đang bị ban | Hiển thị thông báo lỗi |
| 400 | Can Only Assign Lecturer To Event | User không phải Lecturer | Hiển thị thông báo lỗi |
| 400 | Staff Cannot Assign Staff Role | EventRole là `Staff` | Hiển thị thông báo lỗi |
| 400 | Invalid EventRole | EventRole không phải Judge/Mentor | Hiển thị thông báo lỗi |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Chuyển về trang login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Hiển thị thông báo Không có quyền |
| 404 | User Not Found | UserId không tồn tại | Hiển thị thông báo Không tìm thấy |
| 404 | Event Role Not Found | EventRole không tồn tại trong hệ thống | Hiển thị thông báo lỗi |
| 409 | User Is Already Assigned To This Event | Lecturer đã được assign vào event này | Hiển thị thông báo trùng lặp |

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/post/admin.assign.assign-user.md)
