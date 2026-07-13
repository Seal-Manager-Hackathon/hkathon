# GET /api/v1/student/reports

> Student lấy danh sách report của mình, có filter và phân trang.

**Controller:** [StudentReportController.cs](../../../Controllers/Student/StudentReportController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/reports)

## Nghiệp vụ

Student muốn xem lại các report mình đã gửi:
- **Chỉ lấy report của chính user** (lọc theo UserId từ token).
- Hỗ trợ lọc theo:
  - **keyword**: tìm kiếm theo title (chứa chuỗi con, không phân biệt hoa thường).
  - **status**: Pending, Reject, Resolved, Canceled.
  - **fromDate/toDate**: lọc theo thời gian tạo.
- Sắp xếp theo CreatedAt giảm dần (mới nhất trước).
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Student

## Request

| Param | Kiểu | Bắt buộc | Mặc định | Mô tả |
|-------|------|----------|----------|-------|
| keyword | string | ❌ | - | Tìm kiếm theo title |
| status | string | ❌ | - | Enum: Pending, Reject, Resolved, Canceled |
| fromDate | datetime | ❌ | - | Lọc từ ngày |
| toDate | datetime | ❌ | - | Lọc đến ngày |
| pageIndex | int | ❌ | 1 | Trang hiện tại |
| pageSize | int | ❌ | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "title": "Report title",
        "description": "Mô tả chi tiết",
        "status": "Pending",
        "typeReport": "Spam",
        "createdAt": "2026-07-13T10:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status. Must be: Pending, Reject, Resolved, Canceled | Status không hợp lệ |
| 401 | Unauthorized | Token hết hạn/thiếu |
