# POST /api/v1/admin/events/{eventId}/awards

> Admin tạo phần thưởng mới cho event.

## Nghiệp vụ
- Tạo phần thưởng cho event
- LevelAward tự động gán:
  - Nếu chưa có award nào có level=1 → award mới có level=1
  - Nếu đã có → level = max level hiện tại + 1
- NumberOfAward, Prize phải > 0
- Số lượng mặc định là 1
- Tên giải thưởng phải **duy nhất trong cùng event** — không được trùng tên (không phân biệt hoa thường)

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | |
| name | string | ✅ (body) | `Giải Nhất` | Tối đa 200 ký tự |
| description | string | ❌ (body) | `Phần thưởng cho đội đạt giải nhất` | |
| numberOfAward | int | ❌ (body) | `1` | > 0, mặc định 1 |
| prize | decimal | ✅ (body) | `10000000` | > 0 |

### Ví dụ request body
```json
{
  "name": "Giải Nhất",
  "description": "Phần thưởng cho đội đạt giải nhất",
  "numberOfAward": 1,
  "prize": 10000000
}
```

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "name": "Giải Nhất",
    "description": "Phần thưởng cho đội đạt giải nhất",
    "levelAward": 1,
    "numberOfAward": 1,
    "prize": 10000000,
    "isDisable": false
  },
  "message": "Award Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Name Is Required | Thiếu name | Báo "Tên phần thưởng không được để trống" |
| 400 | Award Name Already Exists In This Event | Tên giải thưởng bị trùng trong event | Validation form |
| 400 | NumberOfAward Must Be Greater Than 0 | numberOfAward <= 0 | Báo "Số lượng phải lớn hơn 0" |
| 400 | Prize Must Be Greater Than 0 | prize <= 0 | Báo "Giá trị giải thưởng phải lớn hơn 0" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Resource Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
