# POST /api/v1/staff/register-teams/{registerTeamId}/ban

> Cấm (ban) một đội tham gia event. Team bị banned không thể nộp bài hay tiếp tục tham gia.

## Nghiệp vụ
- Kiểm tra staff có được phân công vào event chứa register team không
- Set `IsBanned = true`, status = `Banned`, lưu `rejectionReason`
- Không cho phép ban nếu team đã bị banned trước đó (IsBanned = true)

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team cần ban |

### Body

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| rejectionReason | string | Có | `Vi phạm quy chế cuộc thi` | Lý do ban |

## Response (200)

```json
{
  "data": null,
  "message": "Ban register team thành công",
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
| 400 | `Register team is already banned` | Team đã bị banned trước đó | Hiển thị thông báo lỗi |
| 401 | `Unauthorized` | Token hết hạn hoặc thiếu | Redirect sang trang login |
| 403 | `Forbidden` | Staff không được phân công vào event | Hiển thị thông báo không có quyền |
| 404 | `Register team not found` | registerTeamId không tồn tại | Hiển thị thông báo không tìm thấy |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-teams/{registerTeamId}/ban) — [`admin/register-team/post/admin.register-teams.ban.md`](../../../admin/register-team/post/admin.register-teams.ban.md)
