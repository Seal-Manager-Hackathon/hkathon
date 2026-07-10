# GET /api/v1/staff/criteria-templates/{templateId}

> Staff xem chi tiết một criteria template (kèm danh sách criteria items).

## Nghiệp vụ
- Staff phải được phân công vào event có chứa round của template này.
- Trả về thông tin template kèm tất cả criteria items.
- Response giống hệt Admin GET /api/v1/admin/criteria-templates/{templateId}.

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| templateId | Guid | ✅ | ID của criteria template |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "roundId": "guid",
    "title": "Tiêu chí vòng loại",
    "description": "Đánh giá ý tưởng và khả thi",
    "isDisable": false,
    "isActive": true,
    "items": [
      {
        "id": "guid",
        "name": "Tính sáng tạo",
        "description": "Ý tưởng có tính mới",
        "score": 30,
        "isDisable": false,
        "createdAt": "..."
      }
    ],
    "createdAt": "...",
    "updatedAt": "..."
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Staff hoặc ko được assign |
| 404 | Criteria Template Not Found | ID không tồn tại |
