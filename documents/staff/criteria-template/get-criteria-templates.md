# GET /api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates — Lấy danh sách criteria template của round

## Mục đích

Staff muốn xem danh sách các tiêu chí chấm điểm (criteria templates) được thiết lập cho một round. Các template này định nghĩa cách chấm điểm cho round đó.

## Business Context

- Criteria template là bộ tiêu chí chấm điểm, mỗi template có nhiều criteria item (mục con)
- Entity dùng field `Title` ánh xạ sang `Name` trong response
- Chỉ trả về template có `IsDisable = false`, nhưng response vẫn trả field `IsDisable`
- Staff phải được phân công vào event

## Endpoint

```
GET /api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates
```

## Controller → Service → Repository

`StaffCriteriaTemplateController.GetCriteriaTemplateByRoundId()` → `ICriteriaTemplateService.GetCriteriaTemplateByRoundId()` → `ICriteriaTemplateRepository.GetByRoundIdAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |
| `roundId` | Guid | Yes | ID của round |

## Response

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "roundId": "guid",
        "name": "Tiêu chí chấm điểm vòng loại",
        "description": "Đánh giá ý tưởng và khả thi",
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ]
  }
}
```

## Exception Handling

| Status | Meaning |
|--------|---------|
| 401 | Token không hợp lệ hoặc đã hết hạn |
| 403 | User không có role Staff hoặc không được phân công vào event |
| 404 | Không tìm thấy round (hoặc round đã bị disable) |
