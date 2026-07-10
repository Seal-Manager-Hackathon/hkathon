# GET /api/v1/staff/tracks/{trackId}/submissions

> Lấy danh sách submission của một track.

## Nghiệp vụ
- Staff chỉ xem được submission của track thuộc event mình được phân công
- Phân trang: PageIndex, PageSize
- Mỗi item trả kèm thông tin team, round, last submission, lịch sử nộp

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| trackId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của track |

### Query Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| pageIndex | int | Không | `1` | Mặc định = 1 |
| pageSize | int | Không | `10` | Mặc định = 10 |

## Response (200)

Giống response của `GET /api/v1/staff/events/{eventId}/submissions`.

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Track Not Found | trackId không tồn tại | Hiển thị thông báo không tìm thấy |

> **Ref:** [Admin API tương ứng](/api/v1/admin/tracks/{trackId}/submissions) — [`admin/submission/get/admin.tracks.submissions.md`](../../../admin/submission/get/admin.tracks.submissions.md)
