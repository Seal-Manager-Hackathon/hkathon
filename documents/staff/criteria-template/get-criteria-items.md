# GET /api/v1/staff/events/{eventId}/criteria-templates/{criteriaTemplateId}/items — Lấy danh sách criteria items

## Mục đích

Staff muốn xem các mục tiêu chí (criteria items) bên trong một criteria template. Các item này là chi tiết từng tiêu chí chấm điểm.

## Business Context

- Mỗi criteria template có nhiều criteria item, mỗi item có tên, mô tả, và điểm tối đa
- Entity `CriteriaItems` dùng field `Score` ánh xạ sang `MaxScore` trong response
- Chỉ trả về item có `IsDisable = false`, nhưng response vẫn trả field `IsDisable`
- Staff phải được phân công vào event

## Endpoint

```
GET /api/v1/staff/events/{eventId}/criteria-templates/{criteriaTemplateId}/items
```

## Controller → Service → Repository

`StaffCriteriaTemplateController.GetCriteriaItemsByTemplateId()` → `ICriteriaTemplateService.GetCriteriaItemsByTemplateId()` → `ICriteriaTemplateRepository.GetByIdWithItemsAsync()`.

## Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `eventId` | Guid | Yes | ID của event |
| `criteriaTemplateId` | Guid | Yes | ID của criteria template |

## Response

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "criteriaTemplateId": "guid",
        "name": "Tính sáng tạo",
        "description": "Ý tưởng có tính mới và sáng tạo",
        "maxScore": 30,
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
| 404 | Không tìm thấy criteria template |
