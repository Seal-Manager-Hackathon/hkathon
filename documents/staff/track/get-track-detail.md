# GET /api/v1/staff/events/{eventId}/tracks/{trackId} — Xem chi tiết track

## Mục đích

Staff muốn xem thông tin chi tiết của track (bao gồm số lượng đội đã đăng ký) để đánh giá mức độ quan tâm.

## Business Context

- Ngoài các thông tin cơ bản, API này trả về `RegisterTeamCount` — số đội đã đăng ký vào track này
- Track bị disable (`IsDisable = true`) sẽ trả về 404
- Staff phải được phân công vào event chứa track

## Endpoint

```
GET /api/v1/staff/events/{eventId}/tracks/{trackId}
```

## Controller → Service → Repository

`StaffTrackController.GetTrackDetail()` → `ITrackService.GetTrackDetail()` → `ITrackRepository.GetByIdAsync()` + `IRegisterTeamRepository.CountByTrackIdAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `trackId` | Guid | Yes | ID của track (không cần eventId trên route — track đã có EventId trong DB) |

## Response

```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "title": "Trí tuệ nhân tạo",
    "description": "Các đề tài về AI",
    "maxTeam": 20,
    "registerTeamCount": 15,
    "isDisable": false,
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
| 404 | Không tìm thấy track (hoặc track đã bị disable) |
