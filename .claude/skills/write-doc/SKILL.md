---
name: write-doc
description: "Use when user asks to write API documentation for FE. Follows standard doc format of the project."
---

# Write API Documentation

## Cấu trúc thư mục

```
Hackathon.Presentation/documents/        ← documents gốc
├── auth/                                 ← controller public (AuthController)
│   └── post/
│       ├── auth.register.md
│       ├── auth.verify-email.md
│       └── auth.login.md
│
└── admin/                                ← controller admin (Admin/)
    ├── user/                             ← AdminUserController
    │   ├── get/
    │   │   ├── admin.users.recent.md     # GET /api/v1/admin/users/recent
    │   │   └── admin.users.count.md      # GET /api/v1/admin/users/count
    │   └── post/
    │       └── admin.users.md            # POST /api/v1/admin/users
    ├── event/                            ← AdminEventController
    │   └── get/
    │       ├── admin.events.recent.md    # GET /api/v1/admin/events/recent
    │       └── admin.events.count.md     # GET /api/v1/admin/events/count
    ├── team/                             ← AdminTeamController
    │   └── get/
    │       └── admin.teams.count.md      # GET /api/v1/admin/teams/count
    └── report/                           ← AdminReportController
        └── get/
            └── admin.reports.recent.md   # GET /api/v1/admin/reports/recent
```

### Quy tắc đặt tên file

Format: `{controller}.{router-path}.md`

| Router | Tên file |
|--------|----------|
| `POST /api/v1/auth/login` | `auth.login.md` |
| `POST /api/v1/auth/register` | `auth.register.md` |
| `GET /api/v1/admin/users/count` | `admin.users.count.md` |
| `GET /api/v1/admin/events/count` | `admin.events.count.md` |
| `POST /api/v1/admin/users` | `admin.users.md` |

---

## Format nội dung mỗi file

### Template đầy đủ

```markdown
# {METHOD} /api/v1/{controller}/{path}

> {Mô tả ngắn gọn chức năng của API}

## Nghiệp vụ
- {Luật nghiệp vụ 1}
- {Luật nghiệp vụ 2}
- {Luật nghiệp vụ 3}

## Phân quyền
- **Public** — không cần token
// hoặc
- ✅ `{Policy}` — chỉ {role} (vd: AdminPolicy — chỉ Admin)

## Request
// Nếu là POST/PUT/PATCH có body:
```json
{ { "field": "value", ... } }
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| {field} | ✅ / ❌ | {mô tả, ví dụ enum} |

// Nếu là GET có query params:
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| {param} | {type} | ✅ / ❌ | {value} | {mô tả} |

// Nếu là GET không params: ghi "Không body, không query params."

## Response ({statusCode})
```json
{ "isSuccess": true, "status": {code}, "message": "{message}", "data": { ... } }
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| {code} | `{errorMessage}` | {nguyên nhân} | {hành động FE} |
```

---

## Checklist khi viết doc

- [ ] Tạo đúng thư mục: `documents/{controller}/{method}/`
- [ ] Đặt tên file: `{controller}.{path}.md`
- [ ] Header: `# {METHOD} /api/v1/{controller}/{path}`
- [ ] Mô tả ngắn: `> {description}`
- [ ] Nghiệp vụ: list business rules
- [ ] Phân quyền: Public hoặc policy gì
- [ ] Request: JSON mẫu + field table (body hoặc query params)
- [ ] Nếu có enum → ⚠️ ghi chú riêng bảng enum values
- [ ] Response: JSON mẫu với status code
- [ ] Lỗi: bảng đủ 4 cột (Status, message, Khi nào, FE xử lý)
- [ ] **Check code để xác nhận nghiệp vụ** — trước khi viết/sửa doc, đọc file Service.cs tương ứng (hoặc Controller.cs) để check:
  - Các validation/exception thực tế trong code có đúng như doc không
  - Request/Response fields trong code có khớp với doc không
  - Business logic ẩn (sort order, filter mặc định, exclude gì) có được ghi đủ không
  - **Nếu sửa API**: đọc doc cũ trước → check diff code → sửa doc cho khớp

---

## Mẫu tham chiếu

Xem các file trong `documents/` để tham khảo:
- `documents/auth/post/auth.login.md`
- `documents/auth/post/auth.register.md`
- `documents/admin/user/get/admin.users.count.md`
- `documents/admin/event/get/admin.events.count.md`
- `documents/admin/user/post/admin.users.md`
- `documents/admin/report/get/admin.reports.recent.md`
