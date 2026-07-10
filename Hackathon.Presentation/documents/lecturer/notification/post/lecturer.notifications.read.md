# POST /api/v1/lecturer/notifications/{notificationId}/read

> Lecturer đánh dấu một thông báo là đã đọc.

## Nghiệp vụ
- Chuyển `Status` từ `Unread` → `Read`.
- Chỉ được đánh dấu thông báo System hoặc Personal của mình.

## Phân quyền
- ✅ Lecturer

## Request
| Param | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| notificationId | Guid | ✅ | ID thông báo |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Lecturer hoặc ko có quyền |
| 404 | Notification Not Found | ID không tồn tại |

> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.
