# Update 06 — Đồng bộ Staff APIs với Admin (routes + response names)

> Sửa route và response field names của Staff API cho khớp Admin. Staff là copy từ Admin, chỉ khác authorization + check assignment.

---

## 1. Round Detail — Route bỏ eventId + thêm EventName

### Route thay đổi

| Trước | Sau |
|-------|-----|
| `GET /api/v1/staff/events/{eventId}/rounds/{roundId}` | `GET /api/v1/staff/rounds/{roundId}` |

### Response thay đổi

| Field | Trước | Sau |
|-------|-------|-----|
| `eventName` | Thiếu | Thêm `"eventName"` |

### Response trước
```json
{
  "data": {
    "id": "guid", "eventId": "guid",
    "name": "Vòng 1", "description": "...",
    "roundNo": 1, "isDisable": false,
    "createdAt": "...", "updatedAt": "..."
  }
}
```

### Response sau
```json
{
  "data": {
    "id": "guid", "eventId": "guid",
    "eventName": "Hackathon AI 2026",
    "name": "Vòng 1", "description": "...",
    "roundNo": 1, "isDisable": false,
    "createdAt": "...", "updatedAt": "..."
  }
}
```

### Files changed
- `Staff/StaffRoundController.cs` — route `events/{eventId}/rounds/{roundId}` → `rounds/{roundId}`, bỏ `eventId` param
- `Staff/Round/IRoundService.cs` — `GetRoundDetail(Guid eventId, Guid roundId)` → `GetRoundDetail(Guid roundId)`
- `Staff/Round/Service.cs` — mapping `EventName`, check assignment qua `round.EventId` thay vì nhận từ route
- `Staff/Round/Response.cs` — thêm `EventName`

### Logic
- **Có chỉnh**: Staff trước đây nhận `eventId` từ route để check assignment. Sau fix, lấy `EventId` từ entity round để check.

---

## 2. Staff Event list — Items → Events

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Response field | `"items"` | `"events"` |

### Response trước
```json
{
  "data": {
    "items": [
      { "id": "guid", "name": "Hackathon", "status": "Published", "isDisable": false, ... }
    ],
    "totalCount": 1, "pageIndex": 1, "pageSize": 10
  }
}
```

### Response sau
```json
{
  "data": {
    "events": [
      { "id": "guid", "name": "Hackathon", "status": "Published", "isDisable": false, ... }
    ],
    "totalCount": 1, "pageIndex": 1, "pageSize": 10
  }
}
```

### Files changed
- `Staff/Event/Response.cs` — `Items` → `Events`
- `Staff/Event/Service.cs` — mapping `Items =` → `Events =` (2 methods: GetMyEvents, GetMyStaffEvents)
- `documents/staff/event/get/*.md` — cập nhật response

### Logic
- Không đổi

---

## 3. CriteriaTemplate list — Items → Templates

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Response field | `"items"` | `"templates"` |

### Response trước
```json
{
  "data": {
    "items": [
      { "id": "guid", "roundId": "guid", "title": "Tiêu chí vòng loại", "isDisable": false, "isActive": true, ... }
    ],
    "totalCount": 1, "pageIndex": 1, "pageSize": 10
  }
}
```

### Response sau
```json
{
  "data": {
    "templates": [
      { "id": "guid", "roundId": "guid", "title": "Tiêu chí vòng loại", "isDisable": false, "isActive": true, ... }
    ],
    "totalCount": 1, "pageIndex": 1, "pageSize": 10
  }
}
```

### Files changed
- `Staff/CriteriaTemplate/Response.cs` — `Items` → `Templates`
- `Staff/CriteriaTemplate/Service.cs` — mapping `Items =` → `Templates =`
- `documents/staff/criteria-template/get/staff.criteria-templates.md` — cập nhật response

### Logic
- Không đổi

---

## Tổng kết

### File thay đổi (code)

| File | Thay đổi |
|------|----------|
| `Staff/StaffRoundController.cs` | Route round detail bỏ eventId |
| `Staff/Round/IRoundService.cs` | Signature GetRoundDetail bỏ eventId |
| `Staff/Round/Service.cs` | Mapping EventName, lấy EventId từ entity |
| `Staff/Round/Response.cs` | Thêm `EventName` |
| `Staff/Event/Response.cs` | `Items` → `Events` |
| `Staff/Event/Service.cs` | `Items =` → `Events =` (2 places) |
| `Staff/CriteriaTemplate/Response.cs` | `Items` → `Templates` |
| `Staff/CriteriaTemplate/Service.cs` | `Items =` → `Templates =` |

### File thay đổi (doc)

| File | Thay đổi |
|------|----------|
| `documents/staff/round/get/staff.rounds.detail.md` | Route mới + response thêm eventName |
| `documents/staff/event/get/staff.events.md` + `staff.events.my-staff.md` | `"items"` → `"events"` |
| `documents/staff/criteria-template/get/staff.criteria-templates.md` | `"items"` → `"templates"` |

### Build
- 0 errors, 0 warnings
