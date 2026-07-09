# GET /api/v1/staff/events/{eventId}/tracks — Lấy danh sách tracks của event

## Mục đích

Staff muốn xem danh sách các track (nhánh thi) của một event để quản lý đề tài. Track là các nhánh chuyên môn trong event (VD: AI, Web, Mobile).

## Business Context

- Mỗi event có nhiều track, mỗi track có nhiều topic
- Chỉ trả về track có `IsDisable = false`, nhưng response vẫn trả field `IsDisable`
- Sắp xếp theo `CreatedAt` giảm dần
- Staff phải được phân công vào event

## Endpoint

```
GET /api/v1/staff/events/{eventId}/tracks
```

## Controller → Service → Repository

`StaffTrackController.GetTracks()` → `ITrackService.GetTracks()` → `ITrackRepository.GetByEventIdAsync()`. Kiểm tra assignment trước.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |

## Request Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Keyword` | string | No | Tìm kiếm theo tên track |
| `PageIndex` | int | No (mặc định 1) | Trang hiện tại |
| `PageSize` | int | No (mặc định 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "eventId": "guid",
        "title": "Trí tuệ nhân tạo",
        "description": "Các đề tài về AI",
        "maxTeam": 20,
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched successfully",
  "traceId": "..."
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff hoặc không được phân công vào event |
