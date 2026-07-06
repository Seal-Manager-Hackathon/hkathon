---
name: exception-handling-check
description: "Sau khi tạo API phải kiểm tra kỹ các exception cases có thể xảy ra"
metadata:
  type: feedback
---

Bước cuối cùng sau khi tạo API: check tất cả các lỗi có thể xảy ra.
- 404: không tìm thấy entity
- 400: enum sai, validation fail, đã disable rồi, ...
- 409: conflict (email trùng, ...)
- 401: token hết hạn
- 403: không có quyền

**Why:** Tránh crash không kiểm soát, FE nhận được error message rõ ràng.
**How to apply:** Step 12 trong create-api skill — trước build.
