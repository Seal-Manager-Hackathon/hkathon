# PATCH /api/v1/admin/awards/{awardId}

> Admin cập nhật thông tin phần thưởng.

## Nghiệp vụ
- Cập nhật các field của phần thưởng (chỉ gửi field cần sửa)
- Không cho sửa LevelAward (level tự động gán khi tạo)
- NumberOfAward, Prize nếu gửi lên phải > 0

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| awardId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | |
| name | string | ❌ (body) | `Giải Nhất` | |
| description | string | ❌ (body) | `Phần thưởng cho đội đạt giải nhất` | |
| numberOfAward | int | ❌ (body) | `1` | > 0 |
| prize | decimal | ❌ (body) | `10000000` | > 0 |
| isDisable | bool | ❌ (body) | `false` | |

### Ví dụ request body
```json
{
  "name": "Giải Nhất",
  "prize": 12000000
}
```

## Response (200)
```json
{
  "data": null,
  "message": "Award Updated Successfully",
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
| 400 | NumberOfAward Must Be Greater Than 0 | numberOfAward <= 0 | Báo "Số lượng phải lớn hơn 0" |
| 400 | Prize Must Be Greater Than 0 | prize <= 0 | Báo "Giá trị giải thưởng phải lớn hơn 0" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Resource Not Found | awardId không tồn tại | Báo "Không tìm thấy" |
