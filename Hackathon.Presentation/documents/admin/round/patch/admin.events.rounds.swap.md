# PATCH /api/v1/admin/events/{eventId}/rounds/{roundId}/swap

> Admin swap (đổi chỗ) round hiện tại với round khác trong cùng event.

## Nghiệp vụ
- Chọn round hiện tại (roundId) và targetRoundNo để swap
- Chỉ đổi các thông tin:
  - ✅ `roundNo` — đổi số thứ tự cho nhau
  - ✅ `startTime`, `endTime` — đổi thời gian
  - ✅ `startSubmission`, `endSubmission` — đổi thời gian nộp bài
- ❌ **Không đổi:** `name`, `description`, `limitTeam`
- 404 nếu target roundNo không tồn tại trong event
- 400 nếu chọn swap với chính nó

## Phân quyền
- ✅ Admin

## Request
```json
{
  "targetRoundNo": 2
}
```

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| targetRoundNo | int | ✅ (body) | `2` |

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
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
| 400 | Cannot Swap Round With Itself | roundId trùng với round của targetRoundNo | Báo "Không thể swap với chính nó" |
| 400 | Round No Not Found In Event | targetRoundNo không tồn tại trong event | Báo "Số thứ tự round không tồn tại" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
| 404 | Round Not Found | roundId không tồn tại hoặc ko thuộc event | Báo "Không tìm thấy round" |
