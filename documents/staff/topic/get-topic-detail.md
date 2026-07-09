# GET /api/v1/staff/topics/{topicId} — Xem chi tiết topic

## Mục đích

Staff muốn xem thông tin chi tiết của một đề tài (topic) cụ thể.

## Business Context

- Topic bị disable (`IsDisable = true`) sẽ trả về 404
- Staff phải được phân công vào event chứa track của topic này
- Response trả thêm `TrackTitle` để FE hiển thị context

## Endpoint

```
GET /api/v1/staff/topics/{topicId}
```

## Controller → Service → Repository

`StaffTopicController.GetTopicDetail()` → `ITopicService.GetTopicDetail()` → `ITopicRepository.GetByIdAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `topicId` | Guid | Yes | ID của topic |

## Response

```json
{
  "data": {
    "id": "guid",
    "trackId": "guid",
    "trackTitle": "Trí tuệ nhân tạo",
    "title": "Xây dựng Chatbot thông minh",
    "description": "Xây dựng chatbot sử dụng NLP",
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
| 404 | Không tìm thấy topic (hoặc topic đã bị disable) |
