# Update 07 — Fix Staff routes: Detail APIs bỏ eventId

> Các API detail của Staff không cần eventId trong route (vì entity đã có EventId). Khi detail entity, chỉ cần id của entity đó.

---

## 1. Award Detail

### Route thay đổi
| Trước | Sau |
|-------|-----|
| `GET /api/v1/staff/events/{eventId}/awards/{awardId}` | `GET /api/v1/staff/awards/{awardId}` |

### Files changed
- `Staff/StaffAwardController.cs` — route + bỏ eventId param
- `Staff/Award/IAwardService.cs` — signature
- `Staff/Award/Service.cs` — lấy EventId từ award.EventId để check assign

### Response
- Không đổi (giống Admin)

---

## 2. Track Detail

### Route thay đổi
| Trước | Sau |
|-------|-----|
| `GET /api/v1/staff/events/{eventId}/tracks/{trackId}` | `GET /api/v1/staff/tracks/{trackId}` |

### Files changed
- `Staff/StaffTrackController.cs` — route, bỏ eventId param

### Response
- Không đổi

---

## 3. Criteria Templates by Round

### Route thay đổi
| Trước | Sau |
|-------|-----|
| `GET /api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates` | `GET /api/v1/staff/rounds/{roundId}/criteria-templates` |

### Files changed
- `Staff/StaffCriteriaTemplateController.cs` — route + bỏ eventId
- `Staff/CriteriaTemplate/ICriteriaTemplateService.cs` — signature
- `Staff/CriteriaTemplate/Service.cs` — lấy EventId từ round.EventId

---

## 4. Criteria Items by Template

### Route thay đổi
| Trước | Sau |
|-------|-----|
| `GET /api/v1/staff/events/{eventId}/criteria-templates/{templateId}/items` | `GET /api/v1/staff/criteria-templates/{templateId}/items` |

### Files changed
- `Staff/StaffCriteriaTemplateController.cs` — route + bỏ eventId
- `Staff/CriteriaTemplate/ICriteriaTemplateService.cs` — signature
- `Staff/CriteriaTemplate/Service.cs` — lấy EventId từ template → round → event

---

## Tổng kết

| File | Thay đổi |
|------|----------|
| `Staff/StaffAwardController.cs` | Route /2 files service |
| `Staff/Award/IAwardService.cs` | Signature |
| `Staff/Award/Service.cs` | Logic + signature |
| `Staff/StaffTrackController.cs` | Route (service đã đúng) |
| `Staff/StaffCriteriaTemplateController.cs` | Route /2 endpoints |
| `Staff/CriteriaTemplate/ICriteriaTemplateService.cs` | Signature |
| `Staff/CriteriaTemplate/Service.cs` | Logic + signature |

---

## 5. Assign Event Detail

### Route thay đổi
| Trước | Sau |
|-------|-----|
| `GET /api/v1/staff/events/{eventId}/assigned/{assignEventId}` | `GET /api/v1/staff/event-assigns/{assignEventId}` |

### Files changed
- `Staff/StaffAssignController.cs` — route + bỏ eventId
- `Staff/Assign/IAssignService.cs` — signature
- `Staff/Assign/Service.cs` — lấy EventId từ assignEvent.EventId

---

## 6. Thêm API CriteriaTemplate + CriteriaItem detail

> Hai API mới cho Staff, copy từ Admin, có check assignment.

### API endpoints
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/criteria-templates/{templateId}` | Detail criteria template (kèm items) |
| GET | `/api/v1/staff/criteria-items/{itemId}` | Detail criteria item |

### Files changed
- `Staff/CriteriaTemplate/ICriteriaTemplateService.cs` — thêm 2 methods
- `Staff/CriteriaTemplate/Response.cs` — thêm 3 DTOs (GetCriteriaTemplateDetailResponse, GetCriteriaItemDetailResponse, CriteriaTemplateItemDetail)
- `Staff/CriteriaTemplate/Service.cs` — thêm `ICriteriaItemRepository`, implement 2 methods
- `Staff/StaffCriteriaTemplateController.cs` — thêm 2 endpoints

---

## Tổng kết

| File | Thay đổi |
|------|----------|
| `Staff/StaffAwardController.cs` | Route |
| `Staff/Award/IAwardService.cs` | Signature |
| `Staff/Award/Service.cs` | Logic |
| `Staff/StaffTrackController.cs` | Route |
| `Staff/StaffCriteriaTemplateController.cs` | Route + 2 endpoints mới |
| `Staff/CriteriaTemplate/ICriteriaTemplateService.cs` | Signature + 2 methods |
| `Staff/CriteriaTemplate/Response.cs` | 3 DTOs mới |
| `Staff/CriteriaTemplate/Service.cs` | Field mới + 2 methods |
| `Staff/StaffAssignController.cs` | Route |
| `Staff/Assign/IAssignService.cs` | Signature |
| `Staff/Assign/Service.cs` | Logic |

### Build
- 0 errors, 0 warnings
