# GET /api/v1/staff/rounds/{roundId}/submissions

> Lấy danh sách submission của một round.

## Nghiệp vụ
- Staff chỉ xem được submission của round thuộc event mình được phân công
- Hỗ trợ tìm kiếm theo keyword (tên team)
- Phân trang: PageIndex, PageSize
- Mỗi item trả kèm thông tin team, track, topic, last submission, lịch sử nộp

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| roundId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của round |

### Query Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| keyword | string | Không | `Team A` | Tìm theo tên team |
| pageIndex | int | Không | `1` | Mặc định = 1 |
| pageSize | int | Không | `10` | Mặc định = 10 |

## Response (200)

Giống response của `GET /api/v1/staff/events/{eventId}/submissions`.

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Round Not Found | roundId không tồn tại | Hiển thị thông báo không tìm thấy |
