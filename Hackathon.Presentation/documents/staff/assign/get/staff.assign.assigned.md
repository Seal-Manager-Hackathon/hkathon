# GET /api/v1/staff/events/{eventId}/assigned — Lấy danh sách người được phân công

## Mục đích

Staff muốn xem danh sách tất cả người dùng đã được phân công vào event này (gồm cả Staff và Lecturer) để quản lý đội ngũ vận hành.

## Business Context

- Trả về danh sách user đã được assign vào event, kèm thông tin tracks họ được phân công
- Mỗi user có thể được gán vào nhiều track thông qua bảng `AssignTracks`
- Lọc các bản ghi assign có `IsDisable = true` (soft-delete) và track `IsDisable = true`
- Hỗ trợ lọc theo keyword, EventRole, Role (User role), TrackId
- Response trả kèm `AssignTracks` — danh sách track user được phân công

## Endpoint

```
GET /api/v1/staff/events/{eventId}/assigned
```

## Controller → Service → Repository

`StaffAssignController.GetAssignedUsers()` → `IAssignService.GetAssignedUsers()` → `IAssignEventRepository.GetAssignedUsersByEventAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |

## Request Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Keyword` | string | No | Tìm kiếm theo tên/email user |
| `EventRole` | string | No | Lọc theo vai trò trong event: `Mentor`, `Judge`, `Staff` |
| `Role` | string | No | Lọc theo user role: `Admin`, `Staff`, `Student`, `Lecturer` |
| `TrackId` | Guid | No | Lọc theo track được phân công |
| `PageIndex` | int | No (mặc định 1) | Trang hiện tại |
| `PageSize` | int | No (mặc định 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
      {
        "assignEventId": "guid",
        "userId": "guid",
        "email": "user@example.com",
        "firstName": "Nguyễn",
        "lastName": "Văn A",
        "avatarUrl": "https://example.com/avatar.jpg",
        "eventRole": "Judge",
        "assignTracks": [
          {
            "trackId": "guid",
            "title": "Trí tuệ nhân tạo",
            "eventId": "guid"
          }
        ]
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 400 | EventRole hoặc Role không hợp lệ |
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff hoặc không được phân công vào event |
