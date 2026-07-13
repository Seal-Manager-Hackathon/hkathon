# POST /api/v1/student/reports

> Student tạo 1 report mới (báo cáo, tố cáo, góp ý...).

**Controller:** [StudentReportController.cs](../../../Controllers/Student/StudentReportController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/reports)

## Nghiệp vụ

Student muốn gửi report về 1 vấn đề gì đó:
- Tạo report mới với title, description, typeReport.
- Mặc định status = **Pending** — chờ admin/staff xử lý.
- Chỉ có thể tạo report cho chính mình (lấy UserId từ token).

## Phân quyền
- ✅ Student

## Request

### Body
```json
{
  "title": "Report title",
  "description": "Mô tả chi tiết vấn đề",
  "typeReport": "Spam"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| title | ✅ | Tối đa 200 ký tự |
| description | ❌ | - |
| typeReport | ❌ | Phân loại (Spam, Cheating, Other...) |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "title": "Report title",
    "status": "Pending",
    "createdAt": "2026-07-13T10:00:00Z"
  },
  "message": "Report Created Successfully",
  "status": 201,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Title Is Required | Thiếu title |
| 401 | Unauthorized | Token hết hạn/thiếu |
