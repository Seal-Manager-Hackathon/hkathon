# GET /api/v1/staff/criteria-items/{itemId}

> Staff xem chi tiết một criteria item.

## Nghiệp vụ
- Staff phải được phân công vào event có chứa round của criteria item này.
- Response giống hệt Admin GET /api/v1/admin/criteria-items/{itemId}.

## Phân quyền
- ✅ Staff (phải được assign vào event)

## Request
| Param | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| itemId | Guid | ✅ | ID của criteria item |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "criteriaTemplateId": "guid",
    "name": "Tính sáng tạo",
    "description": "Ý tưởng có tính mới",
    "score": 30,
    "isDisable": false,
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
| 404 | Criteria Item Not Found | ID không tồn tại |
