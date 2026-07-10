# GET /api/v1/staff/event-assigns/{assignEventId}

> Staff xem chi tiết một Bản ghi phân công.

## Nghiệp vụ
- Staff phải được phân công vào event tương ứng.
- Trả về thông tin chi tiết assign: user, vai trò, các track được gắn, thời gian tạo/cập nhật.
- Nếu assign bị disable (`IsDisable = true`), API trả về 404.

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| assignEventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của Bản ghi assign trong bảng `AssignEvents` |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "firstName": "Nguyen",
    "lastName": "Van A",
    "avatarUrl": "https://example.com/avatar.jpg",
    "eventRole": "Judge",
    "tracks": [
      {
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "Tri tue nhan tao",
        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "isDisable": false
      }
    ],
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
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
| 404 | Resource Not Found | AssignEventId không tồn tại hoặc đã bị disable | Hiển thị thông báo Không tìm thấy |
