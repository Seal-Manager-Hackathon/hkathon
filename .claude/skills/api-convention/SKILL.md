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
- **Tất cả API GET danh sách CHỈ sắp xếp theo thời gian tạo** — mới nhất → cũ nhất: `.OrderByDescending(x => x.CreatedAt)`
- **Không sort theo field khác** (RoundNo, Name, ...) — chỉ CreatedAt

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

## Validation — Quy tắc chung
- **Chỉ validate những field được yêu cầu bắt buộc** (`[Required]`, `[EmailAddress]`, ...)
- **Không tự ý thêm validate** cho các field optional (Description, ...) — chỉ thêm khi user yêu cầu
- Các field bắt buộc có validate: Password (`[StringLength(100, MinimumLength = 6)]`), Email (`[EmailAddress]`), FirstName/LastName (`[StringLength(50)]`), Title (`[StringLength(200)]`)
- Description là optional → **không có `[StringLength]`** trừ khi được yêu cầu cụ thể

> Tham khảo: [documents/update/update01.md](../../Hackathon.Presentation/documents/update/update01.md)

## Document Naming Convention

Tài liệu API trong `documents/{role}/{entity}/{method}/`:

| API Type | Format | Ví dụ |
|----------|--------|-------|
| List (GET all) | `{role}.{entity}.list.md` | `admin.tracks.list.md` |
| Create (POST) | `{role}.{entity}.create.md` | `admin.tracks.create.md` |
| Detail (GET by id) | `{role}.{entity}.detail.md` | `admin.tracks.detail.md` |
| Update (PATCH) | `{role}.{entity}.update.md` | `admin.tracks.update.md` |
| Delete (POST /delete) | `{role}.{entity}.delete.md` | `admin.tracks.delete.md` |
| Restore (POST /restore) | `{role}.{entity}.restore.md` | `admin.tracks.restore.md` |
| Action (POST) | `{role}.{entity}.{action}.md` | `admin.register-teams.approve.md` |
| Sub-resource list | `{role}.{parent}.{child}.list.md` | `admin.criteria-items.list.md` |

**Nội dung file doc:**
```markdown
# {METHOD} /api/v1/{prefix}/{entity}/{action}

> Mô tả ngắn.

## Nghiệp vụ
- Liệt kê logic chính

## Phân quyền
- ✅ {Role}

## Request
{params table / json body}

## Response (200)
```json
{...}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
```
