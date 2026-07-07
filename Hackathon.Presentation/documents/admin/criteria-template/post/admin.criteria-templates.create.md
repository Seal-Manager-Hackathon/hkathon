# POST /api/v1/admin/rounds/{roundId}/criteria-templates

> Admin tạo criteria template mới cho round, kèm danh sách criteria items.

## Phân quyền
- ✅ Admin

## Request
```json
{
  "title": "Đánh giá ý tưởng",
  "description": "Tiêu chí đánh giá ý tưởng vòng 1",
  "items": [
    {
      "name": "Tính sáng tạo",
      "description": "Mức độ mới lạ",
      "score": 25
    },
    {
      "name": "Tính khả thi",
      "score": 25
    }
  ]
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| title | ✅ | Tối đa 200 ký tự |
| description | ❌ | |
| items | ✅ | Tối thiểu 1 item |
| items[].name | ✅ | |
| items[].description | ❌ | |
| items[].score | ✅ | >= 0 |

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Request Data | Validation lỗi |
| 404 | Resource Not Found | RoundId không tồn tại |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | Không phải Admin |
