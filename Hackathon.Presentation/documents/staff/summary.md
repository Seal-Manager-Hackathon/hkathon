# Staff API — Tổng quan

## Vai trò Staff

Staff là người được phân công vào một event để hỗ trợ quản lý và vận hành. Staff **không phải** là Admin — chỉ có thể xem và thao tác trên các event mà họ được phân công (thông qua bảng `AssignEvents`).

## Nguyên tắc xác thực và phân quyền

Tất cả API Staff đều yêu cầu:
1. **JWT token hợp lệ** — xác thực qua `Authorization: Bearer {token}`
2. **Role = Staff** — kiểm tra qua `IAuthorizationService.Authorize(RoleEnum.Staff)`
3. **Event assignment** — kiểm tra user hiện tại có trong `AssignEvents` với `EventId` tương ứng không
4. **Soft-delete filter** — dữ liệu có `IsDisable = true` được ẩn khỏi danh sách, nhưng response vẫn trả field `IsDisable` để FE biết trạng thái

## Danh sách API

### Event
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events` | [Lấy danh sách event được phân công](event/get-my-events.md) |
| GET | `/api/v1/staff/events/{eventId}` | [Xem chi tiết một event được phân công](event/get-my-event-detail.md) |

### Round
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/rounds` | [Lấy danh sách round của event](round/get-rounds.md) |
| GET | `/api/v1/staff/events/{eventId}/rounds/{roundId}` | [Xem chi tiết round](round/get-round-detail.md) |

### Track
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/tracks` | [Lấy danh sách track của event](track/get-tracks.md) |
| GET | `/api/v1/staff/events/{eventId}/tracks/{trackId}` | [Xem chi tiết track](track/get-track-detail.md) |

### Topic
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/tracks/{trackId}/topics` | [Lấy danh sách topic của track](topic/get-topics.md) |
| GET | `/api/v1/staff/topics/{topicId}` | [Xem chi tiết topic](topic/get-topic-detail.md) |

### Criteria Template
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates` | [Lấy danh sách criteria template của round](criteria-template/get-criteria-templates.md) |
| GET | `/api/v1/staff/events/{eventId}/criteria-templates/{criteriaTemplateId}/items` | [Lấy danh sách criteria items của template](criteria-template/get-criteria-items.md) |

### Assign
| Method | Route | Chức năng |
|--------|-------|-----------|
| GET | `/api/v1/staff/events/{eventId}/lecturers/available` | [Lấy danh sách Lecturer có thể phân công](assign/get-available-lecturers.md) |
| POST | `/api/v1/staff/events/{eventId}/assign/users` | [Phân công Lecturer vào event](assign/assign-lecturer.md) |
| GET | `/api/v1/staff/events/{eventId}/assigned` | [Lấy danh sách người được phân công vào event](assign/get-assigned-users.md) |
| GET | `/api/v1/staff/events/{eventId}/assigned/{assignEventId}` | [Xem chi tiết một phân công](assign/get-assign-event-detail.md) |
