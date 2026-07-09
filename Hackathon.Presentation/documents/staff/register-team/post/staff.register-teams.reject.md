# POST /api/v1/staff/register-teams/{registerTeamId}/reject

> Từ chối (reject) một đội đang ở trạng thái `Pending`.

## Nghiệp vụ
- Chỉ cho phép reject khi register team có status = `Pending`
- Set status = `Rejected` và lưu `rejectionReason`
- Nếu team không còn register team nào ở trạng thái `Approved` ở bất kỳ event nào → mở khóa team (set `CanEdit = true`)

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team cần từ chối |

### Body

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| rejectionReason | string | Có | `Thiếu thông tin đăng ký` | Lý do từ chối |

## Response (200)

```json
{
  "data": null,
  "message": "Từ chối register team thành công",
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
| 400 | `Register team is not in Pending status` | Team không ở trạng thái Pending | Hiển thị thông báo lỗi |
| 401 | `Unauthorized` | Token hết hạn hoặc thiếu | Redirect sang trang login |
| 403 | `Forbidden` | Staff không được phân công vào event | Hiển thị thông báo không có quyền |
| 404 | `Register team not found` | registerTeamId không tồn tại | Hiển thị thông báo không tìm thấy |
