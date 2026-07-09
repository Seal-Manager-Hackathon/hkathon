# GET /api/v1/staff/tracks/{trackId}/topics — Lấy danh sách topics của track

## Mục đích

Staff muốn xem danh sách các đề tài (topic) thuộc một track. Mỗi track có thể có nhiều topic để đội thi chọn.

## Business Context

- Topic là đề tài cụ thể trong một track (VD: track AI có topics: "Chatbot", "Computer Vision")
- Chỉ trả về topic có `IsDisable = false`, nhưng response vẫn trả field `IsDisable`
- Sắp xếp theo `CreatedAt` giảm dần
- Staff phải được phân công vào event chứa track

## Endpoint

```
GET /api/v1/staff/tracks/{trackId}/topics
```

## Controller → Service → Repository

`StaffTopicController.GetTopics()` → `ITopicService.GetTopics()` → `ITopicRepository.SearchAsync()`. Kiểm tra staff có được phân công vào event của track hay không.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `trackId` | Guid | Yes | ID của track |

## Request Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Keyword` | string | No | Tìm kiếm theo tên topic |
| `PageIndex` | int | No (mặc định 1) | Trang hiện tại |
| `PageSize` | int | No (mặc định 10) | Số lượng item mỗi trang |

## Response

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "trackId": "guid",
        "trackTitle": "Trí tuệ nhân tạo",
        "title": "Xây dựng Chatbot thông minh",
        "description": "Xây dựng chatbot sử dụng NLP",
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 5,
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
| 404 | Không tìm thấy track |
