# Update 05 — Staff API response alignment với Admin

> Đồng bộ response DTO của Staff API cho khớp với Admin API (cùng field names, cấu trúc, và response shape). Staff API là copy từ Admin, chỉ khác authorization role.

---

## 1. Track — GET /api/v1/staff/events/{eventId}/tracks

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Response.Items | `"items"` | `"tracks"` |

### Response trước
```json
{
  "data": {
    "items": [
      { "id": "guid", "eventId": "guid", "title": "AI", "maxTeam": 20, "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ],
    "totalCount": 1, "pageIndex": 1, "pageSize": 10
  }
}
```

### Response sau
```json
{
  "data": {
    "tracks": [
      { "id": "guid", "eventId": "guid", "title": "AI", "maxTeam": 20, "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ],
    "totalCount": 1, "pageIndex": 1, "pageSize": 10
  }
}
```

### Files changed
- `Hackathon.Application/Services/Staff/Track/Response.cs` — `Items` → `Tracks`
- `Hackathon.Application/Services/Staff/Track/Service.cs` — mapping `Items =` → `Tracks =`
- `documents/staff/track/get/staff.tracks.md` — `"items"` → `"tracks"`

### Logic
- Không đổi

---

## 2. Topic — GET /api/v1/staff/tracks/{trackId}/topics

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Response.Items | `"items"` | `"topics"` |

### Response trước
```json
{
  "data": {
    "items": [
      { "id": "guid", "trackId": "guid", "trackTitle": "AI", "title": "Chatbot", "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ],
    "totalCount": 5, "pageIndex": 1, "pageSize": 10
  }
}
```

### Response sau
```json
{
  "data": {
    "topics": [
      { "id": "guid", "trackId": "guid", "trackTitle": "AI", "title": "Chatbot", "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ],
    "totalCount": 5, "pageIndex": 1, "pageSize": 10
  }
}
```

### Files changed
- `Hackathon.Application/Services/Staff/Topic/Response.cs` — `Items` → `Topics`
- `Hackathon.Application/Services/Staff/Topic/Service.cs` — mapping `Items =` → `Topics =`
- `documents/staff/topic/get/staff.topics.md` — `"items"` → `"topics"`

### Logic
- Không đổi

---

## 3. Criteria Template — GET /api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Field name | `"name"` | `"title"` (field entity `Title` → response) |
| Thiếu | — | `"isActive"` (bool) |
| Thiếu | — | Pagination: `totalCount`, `pageIndex`, `pageSize` |

### Response trước
```json
{
  "data": {
    "items": [
      { "id": "guid", "roundId": "guid", "name": "Tiêu chí vòng loại", "description": "...", "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ]
  }
}
```

### Response sau
```json
{
  "data": {
    "items": [
      { "id": "guid", "roundId": "guid", "title": "Tiêu chí vòng loại", "description": "...", "isDisable": false, "isActive": true, "createdAt": "...", "updatedAt": "..." }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

### Files changed
- `Hackathon.Application/Services/Staff/CriteriaTemplate/Response.cs` — `Name` → `Title`, thêm `IsActive`, thêm pagination fields
- `Hackathon.Application/Services/Staff/CriteriaTemplate/Service.cs` — mapping `Name = t.Title` → `Title = t.Title`, thêm `IsActive`, thêm pagination
- `documents/staff/criteria-template/get/staff.criteria-templates.md` — `"name"` → `"title"`, thêm `"isActive"`, thêm pagination

### Logic
- Không đổi

---

## 4. Criteria Items — GET /api/v1/staff/events/{eventId}/criteria-templates/{criteriaTemplateId}/items

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Field name | `"maxScore"` | `"score"` (field entity `Score` → response) |
| Type | `decimal?` (nullable) | `decimal` (non-nullable) |
| Thiếu | — | Pagination: `totalCount`, `pageIndex`, `pageSize` |

### Response trước
```json
{
  "data": {
    "items": [
      { "id": "guid", "criteriaTemplateId": "guid", "name": "Sáng tạo", "maxScore": 30, "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ]
  }
}
```

### Response sau
```json
{
  "data": {
    "items": [
      { "id": "guid", "criteriaTemplateId": "guid", "name": "Sáng tạo", "score": 30, "isDisable": false, "createdAt": "...", "updatedAt": "..." }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

### Files changed
- `Hackathon.Application/Services/Staff/CriteriaTemplate/Response.cs` — `MaxScore` → `Score` (decimal)
- `Hackathon.Application/Services/Staff/CriteriaTemplate/Service.cs` — `MaxScore = ci.Score` → `Score = ci.Score`, thêm pagination
- `documents/staff/criteria-template/get/criteria-items/staff.criteria-items.md` — `"maxScore"` → `"score"`, thêm pagination

### Logic
- Không đổi

---

## 5. Assign — GET /api/v1/staff/events/{eventId}/assigned

### Thay đổi

| Item | Trước | Sau |
|------|-------|-----|
| Response.AssignedUserItem | Thiếu `isDisable` | Thêm `"isDisable"` |
| Response.AssignedTrackItem | Thiếu `isDisable` | Thêm `"isDisable"` |
| Service | `.Where(at => !at.IsDisable)` filter tracks | Bỏ Where, trả về `IsDisable` luôn |

### Response trước
```json
{
  "data": {
    "items": [
      {
        "assignEventId": "guid", "userId": "guid", "email": "user@example.com",
        "firstName": "Nguyen", "lastName": "Van A", "avatarUrl": "url", "eventRole": "Judge",
        "assignTracks": [
          { "trackId": "guid", "title": "Tri tue nhan tao", "eventId": "guid" }
        ]
      }
    ],
    "totalCount": 5, "pageIndex": 1, "pageSize": 10
  }
}
```

### Response sau
```json
{
  "data": {
    "items": [
      {
        "assignEventId": "guid", "userId": "guid", "email": "user@example.com",
        "firstName": "Nguyen", "lastName": "Van A", "avatarUrl": "url", "eventRole": "Judge",
        "isDisable": false,
        "assignTracks": [
          { "trackId": "guid", "title": "Tri tue nhan tao", "eventId": "guid", "isDisable": false }
        ]
      }
    ],
    "totalCount": 5, "pageIndex": 1, "pageSize": 10
  }
}
```

### Files changed
- `Hackathon.Application/Services/Staff/Assign/Response.cs` — thêm `IsDisable` vào `AssignedUserItem` và `AssignedTrackItem`
- `Hackathon.Application/Services/Staff/Assign/Service.cs` — mapping `IsDisable` cho user, bỏ filter `.Where(!at.IsDisable)` cho tracks, mapping `IsDisable` cho tracks
- `documents/staff/assign/get/staff.assign.assigned.md` — thêm `"isDisable"` vào user item và track item
- `documents/staff/assign/get/staff.assign.assigned.detail.md` — thêm `"isDisable"` vào track item

### Logic
- **Có chỉnh**: Trước đây Staff GET assigned chỉ trả về track đang active (`Where(!at.IsDisable)`). Sau khi fix, trả về tất cả tracks kèm `isDisable` để FE tự quyết định filter. Admin đã làm theo cách này. Đồng bộ Staff theo Admin.

---

## 6. Criteria Template — Service mapping Name → Title

Chi tiết: field entity `CriteriaTemplate.Title` trước đây được map thành `Name` trong response, nay map đúng thành `Title`.

### Files changed
- `Hackathon.Application/Services/Staff/CriteriaTemplate/Service.cs` — `Name = t.Title` → `Title = t.Title`

---

## Tổng kết

### Danh sách file thay đổi (code)

| File | Thay đổi |
|------|----------|
| `Staff/Track/Response.cs` | `Items` → `Tracks` |
| `Staff/Track/Service.cs` | mapping theo tên mới |
| `Staff/Topic/Response.cs` | `Items` → `Topics` |
| `Staff/Topic/Service.cs` | mapping theo tên mới |
| `Staff/CriteriaTemplate/Response.cs` | `Name` → `Title`, thêm `IsActive`, `MaxScore` → `Score`, thêm pagination DTOs |
| `Staff/CriteriaTemplate/Service.cs` | mapping theo DTO mới, thêm pagination |
| `Staff/Assign/Response.cs` | thêm `IsDisable` |
| `Staff/Assign/Service.cs` | mapping `IsDisable`, bỏ filter `.Where(!at.IsDisable)` |

### Danh sách file thay đổi (doc)

| File | Thay đổi |
|------|----------|
| `documents/staff/track/get/staff.tracks.md` | `"items"` → `"tracks"` |
| `documents/staff/topic/get/staff.topics.md` | `"items"` → `"topics"` |
| `documents/staff/criteria-template/get/staff.criteria-templates.md` | `"name"` → `"title"`, thêm `"isActive"`, thêm pagination |
| `documents/staff/criteria-template/get/criteria-items/staff.criteria-items.md` | `"maxScore"` → `"score"`, thêm pagination |
| `documents/staff/assign/get/staff.assign.assigned.md` | thêm `"isDisable"` |
| `documents/staff/assign/get/staff.assign.assigned.detail.md` | thêm `"isDisable"` |

### Logic thay đổi

Chỉ 1 chỗ có logic thay đổi: **Staff GET assigned users** — bỏ filter track active, trả về hết kèm `isDisable` để FE tự quyết định (giống Admin). Còn lại chỉ đổi tên field response cho khớp với Admin.

### Build
- 0 errors, 0 warnings
