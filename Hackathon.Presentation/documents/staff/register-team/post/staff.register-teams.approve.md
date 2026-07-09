# POST /api/v1/staff/register-teams/{registerTeamId}/approve

> Duyệt (approve) một đội đã đăng ký tham gia event. Chỉ duyệt được team đang ở trạng thái `Pending`.

## Nghiệp vụ
- Chỉ cho phép approve khi register team có status = `Pending`
- Kiểm tra round 1 còn slot không (nếu số lượng current >= limit thì từ chối)
- Set status = `Approved`
- Khóa team: set `CanEdit = false` (team không được sửa thông tin sau khi được duyệt)
- Tự động tạo RoundDetail cho round đầu tiên

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team cần duyệt |

## Response (200)

```json
{
  "data": null,
  "message": "Duyệt register team thành công",
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
| 400 | `Round 1 is full` | Round 1 đã đủ số lượng teams | Hiển thị thông báo round đã đầy |
| 401 | `Unauthorized` | Token hết hạn hoặc thiếu | Redirect sang trang login |
| 403 | `Forbidden` | Staff không được phân công vào event | Hiển thị thông báo không có quyền |
| 404 | `Register team not found` | registerTeamId không tồn tại | Hiển thị thông báo không tìm thấy |
