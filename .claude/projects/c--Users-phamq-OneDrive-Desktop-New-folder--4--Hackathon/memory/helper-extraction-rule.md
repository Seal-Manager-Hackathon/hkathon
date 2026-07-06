---
name: helper-extraction-rule
description: "Logic lặp lại ≥2 nơi → tách helper class"
metadata:
  type: feedback
---

Nếu logic giống i chang nhau 100% xuất hiện ở ≥2 nơi trong code (vd: pagination validation, date filter pattern), tách vào helper class:
- Application layer: `Hackathon.Application/Common/Helpers/{Name}Helper.cs`
- Infrastructure layer: `Hackathon.Infrastructure/Helpers/{Name}Helper.cs`

Ví dụ: `PaginationHelper.Validate(pageIndex, pageSize)` dùng chung ở User, Notification, Team services.

**Why:** DRY, dễ maintain, tránh copy-paste sai.
**How to apply:** Bước 8 trong create-api skill — helper check.
