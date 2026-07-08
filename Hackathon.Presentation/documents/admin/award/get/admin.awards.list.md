# GET /api/v1/admin/events/{eventId}/awards

> Admin lấy danh sách awards của event, phân trang, có thể search theo tên và lọc theo disable.

## Nghiệp vụ

- Trả về danh sách giải thưởng của 1 event
- Sắp xếp theo LevelAward tăng dần: giải Nhất (level 1) lên trên cùng, giải thấp hơn ở dưới, giải bị disable (level 0) ở cuối cùng
- Có thể lọc:
  - `keyword`: tìm kiếm theo tên award (contains, không phân biệt hoa thường)
  - `isDisable`: lọc theo trạng thái disable
- Phân trang: mặc định pageIndex=1, pageSize=10
- 404 nếu eventId không tồn tại

## Phân quyền

- ✅ Admin

## Request

| Param     | Kiểu   | Bắt buộc   | Ví dụ                                  |
| --------- | ------ | ---------- | -------------------------------------- |
| eventId   | guid   | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| keyword   | string | ❌ (query) | `Giải nhất`                            |
| isDisable | bool   | ❌ (query) | `false`                                |
| pageIndex | int    | ❌ (query) | `1`                                    |
| pageSize  | int    | ❌ (query) | `10`                                   |

## Response (200)

```json
{
  "data": {
    "awards": [
      {
        "id": "guid",
        "eventId": "guid",
        "name": "Giải Nhất",
        "description": "Phần thưởng cho đội đạt giải nhất",
        "levelAward": 1,
        "numberOfAward": 1,
        "prize": 10000000,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Awards Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi

| Status | message                                           | Khi nào                 | FE xử lý                            |
| ------ | ------------------------------------------------- | ----------------------- | ----------------------------------- |
| 400    | Page Index Must Be Greater Than Zero              | pageIndex < 1           | Báo "Trang không hợp lệ"            |
| 400    | Page Size Must Be Between 1 And 100               | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401    | Invalid Or Expired Token                          | Token hết hạn/thiếu     | Redirect login                      |
| 403    | You do not have permission to perform this action | Không phải Admin        | Ẩn chức năng                        |
| 404    | Resource Not Found                                | eventId không tồn tại   | Báo "Không tìm thấy sự kiện"        |
