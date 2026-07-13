# GET /api/v1/student/users/search

> Tìm kiếm user khác theo email.

## Nghiệp vụ

Student muốn tìm kiếm user khác để kết bạn, mời vào team, v.v.:
- **Chỉ tìm kiếm bằng email** (không search theo tên).
- Chỉ lấy user có `IsDisable = false`.
- Kết quả sắp xếp theo thời gian tạo giảm dần (mới nhất trước).
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Public (không cần đăng nhập)

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Email cần tìm (tìm chuỗi con, không phân biệt hoa thường) |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "users": [
      {
        "id": "guid",
        "email": "user@email.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": null,
        "college": "HUST",
        "studentId": "20210001",
        "isVerified": true,
        "createdAt": "2026-07-01T10:00:00Z"
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

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| users[].id | ID user |
| users[].email | Email |
| users[].firstName | Tên |
| users[].lastName | Họ |
| users[].avatarUrl | Avatar (null nếu chưa có) |
| users[].college | Trường (null nếu chưa có) |
| users[].studentId | Mã số sinh viên (null nếu chưa có) |
| users[].isVerified | Đã xác thực email chưa |
| users[].createdAt | Ngày tạo tài khoản |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Page Index/Size | Pagination sai |
