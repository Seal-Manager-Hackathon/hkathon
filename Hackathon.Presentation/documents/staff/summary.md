# Staff API Documentation

> Tổng hợp tất cả API dành cho role **Staff** trong hệ thống Hackathon.

## Nguyên tắc chung

Staff là người được phân công vào event để hỗ trợ quản lý (không có quyền Admin). Staff có các quyền hạn:

- **Chỉ thao tác trong phạm vi event được phân công** (`AssignEvents` phải có record staff đó).
- **Không thấy dữ liệu bị disable**: tất cả entity rounds, tracks, topics, criteria templates đều filter `IsDisable = false`.
- **Chỉ assign được Lecturer** (Judge/Mentor), không assign được Staff hay Admin.
- **Event mặc định không lấy Draft**: chỉ thấy Published hoặc Closed.

---

## 1. Event APIs

### `GET /api/v1/staff/events`
> Lấy danh sách event mà staff được phân công.

**Filter:** `keyword`, `status`, `fromDate`, `toDate`, `pageIndex`, `pageSize`
**Mặc định:** Không lấy Draft, sắp xếp CreatedAt DESC.

### `GET /api/v1/staff/events/{eventId}`
> Lấy chi tiết 1 event staff được phân công.

---

## 2. Round APIs

### `GET /api/v1/staff/events/{eventId}/rounds`
> Danh sách round của event.

**Filter:** `keyword`, `roundNo`, `pageIndex`, `pageSize`
**Mặc định:** `IsDisable = false`, sắp xếp theo RoundNo ASC.

### `GET /api/v1/staff/events/{eventId}/rounds/{roundId}`
> Chi tiết 1 round. Không trả `IsDisable`.

---

## 3. Criteria Template APIs

### `GET /api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates`
> Lấy danh sách criteria template của round. Chỉ lấy template không bị disable.

### `GET /api/v1/staff/events/{eventId}/criteria-templates/{criteriaTemplateId}/items`
> Lấy danh sách criteria item của 1 template. Chỉ lấy item không bị disable.

---

## 4. Track APIs

### `GET /api/v1/staff/events/{eventId}/tracks`
> Danh sách track của event.

**Filter:** `keyword`, `pageIndex`, `pageSize`
**Mặc định:** `IsDisable = false`, sắp xếp CreatedAt DESC.

### `GET /api/v1/staff/events/{eventId}/tracks/{trackId}`
> Chi tiết 1 track. Không trả `IsDisable`.

---

## 5. Topic APIs

### `GET /api/v1/staff/events/{eventId}/tracks/{trackId}/topics`
> Danh sách topic của track.

**Filter:** `keyword`, `pageIndex`, `pageSize`
**Mặc định:** `IsDisable = false`, sắp xếp CreatedAt DESC.

### `GET /api/v1/staff/events/{eventId}/topics/{topicId}`
> Chi tiết 1 topic. Không trả `IsDisable`.

---

## 6. Assign APIs

### `GET /api/v1/staff/events/{eventId}/lecturers/available`
> Danh sách Lecturer chưa được phân công vào event này.

**Filter:** `keyword`, `pageIndex`, `pageSize`

### `POST /api/v1/staff/events/{eventId}/assign/users`
> Phân công Lecturer vào event với role Judge hoặc Mentor.

**Body:** `{ "userId": "guid", "eventRole": "Judge|Mentor" }`
**Lưu ý:** Staff chỉ assign được Lecturer, không assign được Staff/Admin.

### `GET /api/v1/staff/events/{eventId}/assigned`
> Danh sách user đã được phân công vào event.

**Filter:** `keyword`, `eventRole`, `role`, `trackId`, `pageIndex`, `pageSize`
**Mặc định:** Chỉ lấy record không bị disable (`IsDisable = false`).

### `GET /api/v1/staff/events/{eventId}/assigned/{assignEventId}`
> Chi tiết 1 bản ghi phân công, kèm danh sách track được gán.

---

## Controllers

| Controller | File |
|-----------|------|
| StaffEventController | `Controllers/Staff/StaffEventController.cs` |
| StaffRoundController | `Controllers/Staff/StaffRoundController.cs` |
| StaffTrackController | `Controllers/Staff/StaffTrackController.cs` |
| StaffTopicController | `Controllers/Staff/StaffTopicController.cs` |
| StaffCriteriaTemplateController | `Controllers/Staff/StaffCriteriaTemplateController.cs` |
| StaffAssignController | `Controllers/Staff/StaffAssignController.cs` |

## Services

| Service | Folder |
|---------|--------|
| IEventService | `Services/Staff/Event/` |
| IRoundService | `Services/Staff/Round/` |
| ITrackService | `Services/Staff/Track/` |
| ITopicService | `Services/Staff/Topic/` |
| ICriteriaTemplateService | `Services/Staff/CriteriaTemplate/` |
| IAssignService | `Services/Staff/Assign/` |

## DI Registration

`Services/Staff/DependencyInjection.cs` — `AddStaffServices()` đăng ký tất cả 6 services.
