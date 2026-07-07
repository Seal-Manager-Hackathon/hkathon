---
name: one-controller-per-entity
description: "Mỗi thực thể lớn có controller riêng, không gộp chung"
metadata:
  type: feedback
---

Mỗi entity phải có controller riêng:
- AdminUserController → user APIs
- AdminEventController → event APIs
- AdminTeamController → team APIs
- AdminRegisterTeamController → register-team APIs
- ...

Không gộp nhiều entity vào 1 controller. File đặt tên: `{Role}{Entity}Controller.cs`.

**Why:** Dễ maintain, dễ tìm, mỗi controller chỉ làm 1 việc.
**How to apply:** Khi tạo API mới → entity đã có controller chưa? Chưa thì tạo mới.
