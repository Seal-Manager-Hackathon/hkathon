---
name: api-doc-requirement
description: "Mỗi API mới phải tạo doc, mỗi lần đổi request/response phải cập nhật doc"
metadata:
  type: feedback
---

Mỗi lần tạo API mới → phải tạo file doc trong `documents/{role}/{entity}/{method}/` theo format chuẩn.
Mỗi lần sửa request/response của API → phải cập nhật lại file doc tương ứng cho đúng.

**Why:** Tránh doc lạc hậu, FE dựa vào doc để gọi API.
**How to apply:** Luôn bước "Ghi doc" sau khi viết xong business logic và trước build. Xem template doc tại các file .md có sẵn.
