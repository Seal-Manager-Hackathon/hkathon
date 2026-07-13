# POST /api/v1/student/reports/{reportId}/cancel

> Student hủy 1 report của mình (xóa mềm — chuyển status thành Canceled).

**Controller:** [StudentReportController.cs](../../../Controllers/Student/StudentReportController.cs)

## Nghiệp vụ

Student muốn hủy 1 report mình đã gửi:
- **Xóa mềm**: không delete khỏi DB, chỉ set status = **Canceled**.
- IsDisable vẫn giữ nguyên (false).
- Student vẫn có thể thấy report này trong danh sách (có thể lọc status = Canceled).
- Chỉ được hủy report đang ở trạng thái **Pending**.
- Chỉ hủy được report của chính mình.

## Phân quyền
- ✅ Student

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| reportId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Cannot Cancel a Report That Has Been Processed | Report không còn ở trạng thái Pending |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 404 | Resource Not Found | reportId ko tồn tại hoặc ko phải của user |
