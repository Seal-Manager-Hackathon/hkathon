# GET /api/v1/lecturer/events/{eventId}/assigned

> Lecturer xem thông tin phân công của chính họ trong một event: vai trò (Judge/Mentor) và danh sách track được gán.

## Nghiệp vụ

- Lecturer muốn kiểm tra xem trong event này họ được phân công vai trò gì (Judge hay Mentor) và được gán vào những track nào.
- Lecturer phải được phân công vào event đó thì mới xem được.
- Trả về assignEventId, eventRole, và danh sách tracks kèm trạng thái isDisable.

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer) — dữ liệu chỉ của chính lecturer đang đăng nhập

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của event |

## Response (200)
```json
{
  "data": {
    "assignEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventRole": "Judge",
    "tracks": [
      {
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "Tri tue nhan tao",
        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "isDisable": false
      }
    ]
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
| 403 | You do not have permission to perform this action | Không phải Lecturer | Hiển thị thông báo Không có quyền |
| 404 | Event Not Found or You Are Not Assigned to This Event | EventId không tồn tại hoặc lecturer chưa được phân công | Hiển thị thông báo Không tìm thấy |

> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.
