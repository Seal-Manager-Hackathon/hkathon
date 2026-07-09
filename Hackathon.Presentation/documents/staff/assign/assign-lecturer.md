# POST /api/v1/staff/events/{eventId}/assign/users — Phân công Lecturer vào event

## Mục đích

Staff muốn phân công một Lecturer (giảng viên) vào event với vai trò Judge hoặc Mentor để họ có thể chấm bài hoặc hướng dẫn đội thi.

## Business Context

- **Chỉ có thể assign user có role = Lecturer** (giảng viên) — không thể assign Student, Admin, hay Staff
- **EventRole hợp lệ:** `Judge` (giám khảo) hoặc `Mentor` (cố vấn) — không thể assign role `Staff`
- Một Lecturer chỉ được assign vào một event một lần (kiểm tra duplicate)
- Staff assign dựa trên `UserId` của Lecturer — không assign theo email
- Sau khi assign, Lecturer sẽ xuất hiện trong danh sách assigned của event

## Endpoint

```
POST /api/v1/staff/events/{eventId}/assign/users
```

## Controller → Service → Repository

`StaffAssignController.AssignLecturerToEvent()` → `IAssignService.AssignLecturerToEvent()` → kiểm tra user role, event role, duplicate → tạo `AssignEvents` record.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |

## Request Body

```json
{
  "userId": "guid",
  "eventRole": "Judge"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `userId` | Guid | Yes | ID của Lecturer cần assign |
| `eventRole` | string | Yes | Vai trò trong event: `Judge` hoặc `Mentor` |

## Validation Rules

1. User phải có role = `Lecturer` — nếu không, trả về 400 "Can Only Assign Lecturer To Event"
2. EventRole phải là `Judge` hoặc `Mentor` — nếu là `Staff`, trả về 400 "Staff Cannot Assign Staff Role"
3. User chưa được assign vào event này — nếu đã có, trả về 409 "User Is Already Assigned To This Event"

## Response

```json
{
  "data": null,
  "message": "Created successfully",
  "status": 201,
  "traceId": "..."
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 400 | User không phải Lecturer, hoặc EventRole không hợp lệ, hoặc Staff cố assign role Staff |
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff hoặc không được phân công vào event |
| 404 | Không tìm thấy user hoặc EventRole |
| 409 | User đã được phân công vào event này rồi |
