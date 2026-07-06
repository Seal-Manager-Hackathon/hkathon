# GET /api/v1/user/me

> Lấy thông tin profile của user hiện tại (user đã đăng nhập).

## Nghiệp vụ
- Chỉ cần đăng nhập, không check role
- Không trả về thông tin nhạy cảm: password, banReason, address, dateOfBirth

## Phân quyền
- ✅ Authenticated (bất kỳ user nào đã đăng nhập)

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@fpt.edu.vn",
    "firstName": "Nguyen",
    "lastName": "Van A",
    "avatarUrl": "https://robohash.org/user@fpt.edu.vn",
    "bio": null,
    "role": "Student",
    "status": "Active",
    "isVerified": true,
    "college": "FPT University",
    "createdAt": "2026-07-07T12:00:00Z"
  },
  "message": "Profile Fetched Successfully",
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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 404 | User Not Found | Token hợp lệ nhưng user đã bị xoá | Báo "Không tìm thấy tài khoản" |
