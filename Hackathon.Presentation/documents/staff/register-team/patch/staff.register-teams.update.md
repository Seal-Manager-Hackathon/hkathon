# PATCH /api/v1/staff/register-teams/{registerTeamId}

> Cập nhật thông tin của một register team: description, rejectionReason, status.

## Nghiệp vụ
- Staff cập nhật được các field: `description`, `rejectionReason`, `status`
- Staff **KHÔNG được sửa** `isBanned` và `isDisable` — chỉ Admin mới có quyền này. Các field này nếu gửi lên sẽ bị bỏ qua
- Phải kiểm tra staff có được phân công vào event chứa register team này hay không

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)
- ❌ Staff không được sửa `isBanned`, `isDisable`

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team |

### Body

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| description | string | Không | `Mô tả cập nhật` | Mô tả mới của register team |
| rejectionReason | string? | Không | `null` | Lý do từ chối (có thể null) |
| status | string | Không | `Approved` | Enum: `Pending`, `Approved`, `Rejected`, `Banned` |

## Response (200)

```json
{
  "data": null,
  "message": "Cập nhật register team thành công",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-0b4e4e4b7b8c4d4f8f9a0b1c2d3e4f5a",
  "timestampUtc": "2026-07-09T10:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | `Invalid status value` | Giá trị status không hợp lệ (không thuộc enum) | Hiển thị thông báo lỗi |
| 401 | `Unauthorized` | Token hết hạn hoặc thiếu | Redirect sang trang login |
| 403 | `Forbidden` | Staff không được phân công vào event | Hiển thị thông báo không có quyền |
| 404 | `Register team not found` | registerTeamId không tồn tại | Hiển thị thông báo không tìm thấy |
