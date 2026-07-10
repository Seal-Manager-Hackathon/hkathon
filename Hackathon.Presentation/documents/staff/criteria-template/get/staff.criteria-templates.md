# GET /api/v1/staff/rounds/{roundId}/criteria-templates

> Staff xem danh sách criteria template của round.

## Nghiệp vụ
- Staff phải được phân công vào event tương ứng.
- Chỉ trả về template có `IsDisable = false`.
- Entity `CriteriaTemplate` dùng field `Title` ánh xạ sang `name` trong response.

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của event |
| roundId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của round |

## Response (200)
```json
{
  "data": {
    "templates": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "Tiêu chí chấm điểm vòng loại",
        "description": "Đánh giá ý tưởng và khả thi",
        "isDisable": false,
        "isActive": true,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Chuyển về trang login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Hiển thị thông báo Không có quyền |
| 404 | Resource Not Found | RoundId không tồn tại | Hiển thị thông báo Không tìm thấy |
