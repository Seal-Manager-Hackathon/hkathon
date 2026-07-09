# GET /api/v1/staff/register-teams/{registerTeamId}/submissions

> Lấy danh sách submission của một register team.

## Nghiệp vụ
- Staff chỉ xem được submission của register team thuộc event mình được phân công
- Có thể lọc theo roundId
- Phân trang: PageIndex, PageSize
- Trả về lịch sử nộp bài qua các round

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| registerTeamId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của register team |

### Query Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| roundId | Guid | Không | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | Lọc theo round |
| pageIndex | int | Không | `1` | Mặc định = 1 |
| pageSize | int | Không | `10` | Mặc định = 10 |

## Response (200)

Giống response của `GET /api/v1/staff/events/{eventId}/submissions`.

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId không tồn tại | Hiển thị thông báo không tìm thấy |
