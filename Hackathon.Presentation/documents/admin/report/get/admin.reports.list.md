# GET /api/v1/admin/reports

> Admin lấy danh sách report, có search và filter theo status.

## Nghiệp vụ

- Keyword search trên: email user, fullName (firstName + lastName), title
- Lọc theo status enum: Pending, Approved, Rejected
- Phân trang

## Phân quyền

- ✅ Admin

## Request

| Param     | Kiểu   | Bắt buộc | Mô tả                             |
| --------- | ------ | -------- | --------------------------------- |
| keyword   | string | ❌       | Search email, fullname, title     |
| status    | string | ❌       | Enum: Pending, Approved, Rejected |
| pageIndex | int    | ❌       | Mặc định 1                        |
| pageSize  | int    | ❌       | Mặc định 10, tối đa 100           |

## Response (200)

```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "userId": "guid",
        "userEmail": "user@fpt.edu.vn",
        "userFirstName": "Nguyễn",
        "userLastName": "Văn A",
        "title": "Report title",
        "description": "Mô tả report",
        "status": "Pending",
        "typeReport": "Spam",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message        | Khi nào             |
| ------ | -------------- | ------------------- |
| 400    | Invalid Status | Status sai          |
| 401    | Unauthorized   | Token hết hạn/thiếu |
| 403    | Forbidden      | Không phải Admin    |
