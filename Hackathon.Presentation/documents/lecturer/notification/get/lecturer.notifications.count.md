# GET /api/v1/lecturer/notifications/count

> Lecturer đếm số lượng thông báo dành cho họ (Personal + System).

**Controller:** [LecturerNotificationController.cs](Controllers/Lecturer/LecturerNotificationController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/notifications/count`

- **Không có API tương ứng bên Admin** — API riêng của Lecturer.
- **Personal:** Chỉ đếm notification có `UserId == currentUserId` (của riêng Lecturer đó).
- **System:** Chỉ đếm notification có `TargetType == System`.
- **Luôn filter `IsDisable = false`** — chỉ đếm notification đang hoạt động.

## Phân quyền
- ✅ Lecturer

## Request

Không query params, không body.

## Response (200)
```json
{
  "data": { "total": 15 },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
