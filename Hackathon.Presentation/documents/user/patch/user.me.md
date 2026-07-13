# PATCH /api/v1/user/me

> Cập nhật thông tin cá nhân của user đang đăng nhập.
> Có thể cập nhật cả avatarUrl (URL ảnh). StudentId chỉ được set 1 lần nếu trước đó null.

## Phân quyền
- ✅ Authenticated

## Request

### Body
```json
{
  "firstName": "Nguyen",
  "lastName": "Van A",
  "phoneNumber": "0123456789",
  "bio": "Sinh viên năm 3",
  "address": "Hà Nội",
  "dateOfBirth": "2003-01-15T00:00:00+07:00",
  "studentId": "SE123456",
  "imgUrl": "https://example.com/img.jpg",
  "linkUrl": "https://example.com/profile",
  "avatarUrl": "https://res.cloudinary.com/.../avatar.jpg"
}
```

### Field rules
| Field | Type | Ghi chú |
|-------|------|---------|
| firstName | string? | Bỏ qua nếu null |
| lastName | string? | Bỏ qua nếu null |
| phoneNumber | string? | Bỏ qua nếu null |
| bio | string? | Bỏ qua nếu null |
| address | string? | Bỏ qua nếu null |
| dateOfBirth | datetime? | Bỏ qua nếu null |
| studentId | string? | Chỉ set được khi chưa có dữ liệu. Từ chối nếu đã set trước đó |
| imgUrl | string? | Bỏ qua nếu null |
| linkUrl | string? | Bỏ qua nếu null |
| avatarUrl | string? | URL ảnh đại diện. Bỏ qua nếu null |

## Response (200)
```json
{
  "data": null,
  "message": "Profile Updated Successfully",
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Chưa đăng nhập |
| 404 | User Not Found | User ko tồn tại |
| 400 | Student Id Cannot Be Changed Once Set | StudentId đã được set trước đó |

## Logic
1. Authenticate — lấy userId từ token
2. Load user từ userId
3. Nếu request field != null → update field tương ứng
4. StudentId: nếu request có StudentId → check user.StudentId đã có dữ liệu chưa → nếu rồi thì báo lỗi
5. SaveChanges
