# GET /api/v1/lecturer/notifications/unread-count

> Lecturer lấy số lượng thông báo chưa đọc.

## Nghiệp vụ
- Đếm số thông báo System + Personal có `Status = Unread` dành cho lecturer.
- Dùng để hiển thị badge trên UI.

## Phân quyền
- ✅ Lecturer

## Response (200)
```json
{
  "data": {
    "count": 5
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Lecturer |

> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.
