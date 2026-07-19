# POST /api/v1/student/register-teams

> Student (team leader) đăng ký team tham gia một event.

## Nghiệp vụ

Team leader muốn đăng ký team của mình tham gia một event cụ thể:
- Chỉ leader mới có quyền đăng ký.
- **Số lượng thành viên active trong team phải nằm trong khoảng `[MinMember, MaxMember]` của event** — nếu thiếu/vượt quá sẽ báo lỗi.
- **Khi submit đăng ký, team bị khóa (CanEdit = false)** — không thể sửa thông tin team hoặc kick member sau khi đã gửi đăng ký.
- **Kiểm tra thời hạn đăng ký (RegisterLimitTime):** nếu đã quá hạn → lỗi "Registration Period Has Ended. Cannot Register At This Time."
- Nếu team đã có register team trong event này:
  - Nếu status là **Banned** hoặc IsBanned = true → lỗi "You Have Been Banned From This Event"
  - Nếu status là **Rejected** → reset về **Pending**, xóa RejectionReason, trả về register team cũ (không tạo mới)
  - Nếu status là **Pending** hoặc **Approved** → lỗi "Team Is Already Registered to This Event"
- **Các thành viên active trong team không ai được có approved register team trong event này** (tránh trường hợp 1 người qua nhiều team đăng ký cùng event).
- Event phải đang ở trạng thái **Published** — không cho đăng ký vào Draft hoặc Closed.
- Khi tạo mới, status mặc định là **Pending** — chờ admin/staff duyệt.
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
| 400 | Team Must Have At Least {n} Active Members To Register For This Event | Số member active < MinMember của event |
| 400 | Team Cannot Have More Than {n} Active Members To Register For This Event | Số member active > MaxMember của event |
| 400 | Registration Period Has Ended. Cannot Register At This Time. | Đã quá thời hạn đăng ký (RegisterLimitTime) |
| 400 | You Have Been Banned From This Event | Team đã bị ban khỏi event này |
| 400 | Cannot Register to a Draft or Closed Event | Event đang Draft/Closed |
| 400 | Team Is Already Registered to This Event | Team đã đăng ký event này rồi (Pending hoặc Approved) |
| 400 | One or More Team Members Already Have an Approved Registration in This Event | Thành viên team đã được approved trong event này qua team khác |
| 404 | Team Not Found | Team không tồn tại/bị disable |
| 404 | Event Not Found | Event không tồn tại/bị disable |
