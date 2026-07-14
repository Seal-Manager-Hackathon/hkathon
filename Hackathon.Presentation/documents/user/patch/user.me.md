# PATCH /api/v1/user/me

> Cập nhật thông tin cá nhân của user đang đăng nhập. Hỗ trợ gửi cả fields dạng text và file avatar trong 1 request (multipart/form-data). StudentId chỉ set được 1 lần — nếu đã có rồi thì silently bỏ qua, không throw lỗi.

## Phân quyền
- ✅ Authenticated

## Request

**Content-Type:** `multipart/form-data`

### Fields
| Field | Type | Ghi chú |
|-------|------|---------|
| firstName | string? | Bỏ qua nếu null |
| lastName | string? | Bỏ qua nếu null |
| phoneNumber | string? | Bỏ qua nếu null |
| bio | string? | Bỏ qua nếu null |
| address | string? | Bỏ qua nếu null |
| dateOfBirth | datetime? | Bỏ qua nếu null |
| studentId | string? | Chỉ set được khi chưa có dữ liệu. Nếu đã có rồi thì silently bỏ qua, không throw lỗi |
| imgUrl | string? | Bỏ qua nếu null |
| linkUrl | string? | Bỏ qua nếu null |
| avatarUrl | string? | URL ảnh đại diện. Bỏ qua nếu null |
| avatarFile | file? | Upload file ảnh mới (sẽ upload lên Cloudinary và set avatarUrl). Được ưu tiên hơn avatarUrl nếu gửi cả 2 |

> Gửi `avatarFile` (file) → tự động upload Cloudinary → lưu URL vào `user.AvatarUrl`.
> Gửi `avatarUrl` (string) → set URL trực tiếp (dùng khi đã có sẵn URL từ lần upload trước).

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

## Logic
1. Authenticate — lấy userId từ token
2. Load user từ userId
3. Nếu request field != null → update field tương ứng
4. Nếu có `avatarFile` → upload lên Cloudinary (folder "avatars") → set `user.AvatarUrl` = URL từ Cloudinary
5. StudentId: nếu request có StudentId VÀ user.StudentId hiện tại null → set studentId mới. Nếu user.StudentId đã có rồi → silently bỏ qua, ko throw lỗi
6. SaveChanges
