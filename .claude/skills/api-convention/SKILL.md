# API Convention

## HTTP Methods
| Chức năng | Method |
|-----------|--------|
| Lấy dữ liệu | `GET` |
| Tạo mới | `POST` |
| Xóa mềm | `POST .../delete` |
| Khôi phục | `POST .../restore` |
| Cập nhật thông tin | `PATCH` |
| Các action khác (approve, reject, ban, unban, swap, ...) | `POST` |

- **KHÔNG dùng `PUT`** — tất cả update = `PATCH`
- **KHÔNG dùng `HttpPatch` cho delete/restore/actions** — dùng `HttpPost`

## GET — Sắp xếp
- **Tất cả API GET danh sách phải sắp xếp mới nhất → cũ nhất** theo `CreatedAt`: `.OrderByDescending(x => x.CreatedAt)`

## Soft Delete — Admin
- **Admin soft delete không check gì thêm** — không throw lỗi "already disabled" hay "not disabled"
- Admin gọi delete → set `IsDisable = true`, gọi restore → set `IsDisable = false`
- **Không check trạng thái trước đó** — ai gọi cũng được

## Checklist khi tạo API mới
- [ ] GET: sắp xếp `OrderByDescending(CreatedAt)`
- [ ] POST: tạo mới
- [ ] PATCH: update
- [ ] POST .../delete: xóa mềm (không check already)
- [ ] POST .../restore: khôi phục (không check not disabled)
- [ ] POST: action khác

> Tham khảo: [documents/update/update01.md](../../Hackathon.Presentation/documents/update/update01.md)
