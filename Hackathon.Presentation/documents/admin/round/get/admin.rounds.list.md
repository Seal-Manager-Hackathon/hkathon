# GET /api/v1/admin/events/{eventId}/rounds

> Admin lấy danh sách rounds của event, phân trang, có thể lọc theo keyword (tên round) và roundNo.

## Nghiệp vụ
- Trả về danh sách rounds của 1 event, sắp xếp theo thời gian tạo mới nhất
- Có thể lọc:
  - `keyword`: tìm kiếm theo tên round (contains, không phân biệt hoa thường)
  - `roundNo`: lọc chính xác theo số thứ tự round
  - `isDisable`: lọc theo trạng thái disable
- Phân trang: mặc định pageIndex=1, pageSize=10
- 404 nếu eventId không tồn tại

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| keyword | string | ❌ (query) | `Vòng 1` |
| roundNo | int | ❌ (query) | `1` |
| isDisable | bool | ❌ (query) | `false` |
| pageIndex | int | ❌ (query) | `1` |
| pageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "rounds": [
      {
        "id": "guid",
        "eventId": "guid",
        "name": "Vòng 1",
        "description": "...",
        "roundNo": 1,
        "startTime": "2026-08-01T00:00:00Z",
        "endTime": "2026-08-03T00:00:00Z",
        "startSubmission": "2026-08-03T00:00:00Z",
        "endSubmission": "2026-08-05T00:00:00Z",
        "limitTeam": 20,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Rounds Fetched Successfully",
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
| 400 | Page Index Must Be Greater Than 0 | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
