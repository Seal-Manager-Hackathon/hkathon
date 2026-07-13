# POST /api/v1/student/register-teams

> Student (team leader) đăng ký team tham gia một event.

## Nghiệp vụ

Team leader muốn đăng ký team của mình tham gia một event cụ thể:
- Chỉ leader mới có quyền đăng ký.
- Team chưa được đăng ký vào event này (chỉ được đăng ký 1 lần).
- **Các thành viên active trong team không ai được có approved register team trong event này** (tránh trường hợp 1 người qua nhiều team đăng ký cùng event).
- Event phải đang ở trạng thái **Published** — không cho đăng ký vào Draft hoặc Closed.
- Khi tạo, status mặc định là **Pending** — chờ admin/staff duyệt.
- Có thể chọn Track/Topic ngay khi đăng ký (tuỳ chọn).

## Phân quyền
- ✅ Student (phải là leader của team)

## Request

### Body
```json
{
  "teamId": "guid",
  "eventId": "guid",
  "trackId": "guid",
  "topicId": "guid",
  "description": "Đội em đăng ký tham gia Hackathon 2026"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| teamId | Guid | ✅ | ID của team |
| eventId | Guid | ✅ | ID của event |
| trackId | Guid | ❌ | Track muốn đăng ký |
| topicId | Guid | ❌ | Topic muốn đăng ký |
| description | string | ❌ | Mô tả thêm |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "teamId": "guid",
    "eventId": "guid",
    "status": "Pending",
    "createdAt": "2026-07-13T10:00:00Z"
  },
  "message": "Register Team Created Successfully",
  "status": 201,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 400 | Only Team Leader Can Register Team to Event | User không phải leader |
| 400 | Cannot Register to a Draft or Closed Event | Event đang Draft/Closed |
| 400 | Team Is Already Registered to This Event | Team đã đăng ký event này rồi |
| 400 | One or More Team Members Already Have an Approved Registration in This Event | Thành viên team đã được approved trong event này qua team khác |
| 404 | Team Not Found | Team không tồn tại/bị disable |
| 404 | Event Not Found | Event không tồn tại/bị disable |
