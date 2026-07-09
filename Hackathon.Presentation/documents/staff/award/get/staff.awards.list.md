# GET /api/v1/staff/events/{eventId}/awards

> Staff lấy danh sách awards của event mà họ được phân công, chỉ thấy award đang active (IsDisable = false).

## Nghiệp vụ

- Staff chỉ thấy award có `IsDisable = false` — award đã xóa mềm (disable) không hiển thị
- Staff phải được assign vào event này mới xem được
- Sắp xếp theo LevelAward tăng dần: giải Nhất (level 1) lên trên cùng, giải thấp hơn ở dưới
- Có thể search theo tên award (keyword)
- Phân trang: mặc định pageIndex=1, pageSize=10
- 404 nếu eventId không tồn tại
- 403 nếu staff không được assign vào event

## Phân quyền

- ✅ Staff (phải được assign vào event)

## Request

| Param     | Kiểu   | Bắt buộc   | Ví dụ                                  |
| --------- | ------ | ---------- | -------------------------------------- |
| eventId   | guid   | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| keyword   | string | ❌ (query) | `Giải nhất`                            |
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

| Status | message                                           | Khi nào                            | FE xử lý                            |
| ------ | ------------------------------------------------- | ---------------------------------- | ----------------------------------- |
| 400    | Page Size Must Be Between 1 And 100               | pageSize < 1 hoặc > 100            | Báo "Kích thước trang không hợp lệ" |
| 401    | Invalid Or Expired Token                          | Token hết hạn/thiếu                | Redirect login                      |
| 403    | You Are Not Assigned to This Event                | Staff không được assign vào event  | Ẩn chức năng                        |
| 404    | Resource Not Found                                    | eventId không tồn tại              | Báo "Không tìm thấy sự kiện"      |