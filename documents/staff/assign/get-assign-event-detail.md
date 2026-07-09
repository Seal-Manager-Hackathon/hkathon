# GET /api/v1/staff/events/{eventId}/assigned/{assignEventId} — Xem chi tiết một phân công

## Mục đích

Staff muốn xem chi tiết một bản ghi phân công cụ thể: ai được phân công, vai trò gì, được gán vào những track nào.

## Business Context

- Khác với danh sách, API này trả về thêm `CreatedAt` và `UpdatedAt` của bản ghi assign
- Trả về danh sách `Tracks` — các track mà người này được phân công chấm/hướng dẫn
- Nếu assign bị disable (`IsDisable = true`), API trả về 404
- Chỉ lấy track có `IsDisable = false`

## Endpoint

```
GET /api/v1/staff/events/{eventId}/assigned/{assignEventId}
```

## Controller → Service → Repository

`StaffAssignController.GetAssignEventDetail()` → `IAssignService.GetAssignEventDetail()` → `IAssignEventRepository.GetByIdWithTracksAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |
| `assignEventId` | Guid | Yes | ID của bản ghi assign trong bảng `AssignEvents` |

## Response

```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "userId": "guid",
    "email": "user@example.com",
    "firstName": "Nguyễn",
    "lastName": "Văn A",
    "avatarUrl": "https://example.com/avatar.jpg",
    "eventRole": "Judge",
    "tracks": [
      {
        "trackId": "guid",
        "title": "Trí tuệ nhân tạo",
        "eventId": "guid"
      }
    ],
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  }
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff hoặc không được phân công vào event |
| 404 | Không tìm thấy assign event (hoặc đã bị disable) |
