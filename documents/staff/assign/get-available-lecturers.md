# GET /api/v1/staff/events/{eventId}/lecturers/available — Lấy danh sách Lecturer có thể phân công

## Mục đích

Staff muốn xem danh sách người dùng có role Lecturer (giảng viên) chưa được phân công vào event để có thể thêm họ làm giám khảo (Judge) hoặc cố vấn (Mentor).

## Business Context

- Chỉ trả về user có role = Lecturer
- Chỉ lấy Lecturer chưa được assign vào event này
- Staff chỉ có thể assign Lecturer với EventRole là `Judge` hoặc `Mentor` (không thể assign Staff role)
- API dùng để FE hiển thị dropdown chọn người trước khi thực hiện assign

## Endpoint

```
GET /api/v1/staff/events/{eventId}/lecturers/available
```

## Controller → Service → Repository

`StaffAssignController.GetAvailableLecturers()` → `IAssignService.GetAvailableLecturers()` → `IUserRepository.GetAvailableUsersByRoleAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |

## Request Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Keyword` | string | No | Tìm kiếm theo tên/email lecturer |
| `PageIndex` | int | No (mặc định 1) | Trang hiện tại |
| `PageSize` | int | No (mặc định 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "email": "lecturer@example.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "avatarUrl": "https://example.com/avatar.jpg",
        "college": "Đại học Bách Khoa",
        "phoneNumber": "0909123456"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff hoặc không được phân công vào event |
