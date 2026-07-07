# GET /api/v1/admin/scores/{scoreId}/items

> Admin lấy danh sách chi tiết điểm (score items) của 1 lượt chấm, phân trang.

## Nghiệp vụ
- Trả về danh sách điểm chi tiết từng tiêu chí của 1 lượt chấm
- Phân trang

## Phân quyền
- ✅ Admin

## Request

| Param   | Kiểu | Bắt buộc | Ví dụ |
|---------|------|----------|-------|
| scoreId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

| Param     | Kiểu | Bắt buộc | Mô tả |
|-----------|------|----------|-------|
| pageIndex | int  | ❌        | Mặc định 1 |
| pageSize  | int  | ❌        | Mặc định 10, tối đa 100 |

## Response (200)

```json
{
  "data": {
    "scoreId": "guid",
    "items": [
      {
        "scoreItemId": "guid",
        "criteriaItemId": "guid",
        "criteriaName": "Tính sáng tạo",
        "score": 20,
        "comment": "Ý tưởng tốt"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
