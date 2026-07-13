# PATCH /api/v1/user/me

> Cập nhật thông tin profile của người dùng hiện tại. Hỗ trợ gửi cả text fields và file avatar trong 1 request (multipart/form-data).

**Controller:** [UserController.cs](Controllers/Base/UserController.cs)

## Nghiệp vụ
- Cập nhật các field: firstName, lastName, phoneNumber, bio, address, dateOfBirth, studentId, imgUrl, linkUrl, avatarUrl, avatarFile
- Chỉ cập nhật field có giá trị (nullable)
- **avatarFile** được ưu tiên hơn avatarUrl: nếu gửi cả 2, file sẽ được upload và lưu URL
- Bất kỳ role nào cũng dùng được
- **StudentId** chỉ set được 1 lần (khi đang null)

## Phân quyền
- ✅ Đăng nhập (Mọi role)

## Request

**Content-Type:** `multipart/form-data`

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| firstName | string | Không | `Nguyễn` | Tên |
| lastName | string | Không | `Văn B` | Họ |
| phoneNumber | string | Không | `0909123456` | Số điện thoại |
| bio | string | Không | `Sinh viên năm 3` | Giới thiệu |
| address | string | Không | `Hà Nội` | Địa chỉ |
| dateOfBirth | datetime | Không | `2000-01-15T00:00:00Z` | Ngày sinh |
| studentId | string | Không | `20210001` | Mã sinh viên (set 1 lần) |
| imgUrl | string | Không | `https://example.com/img.jpg` | Ảnh (URL) |
| linkUrl | string | Không | `https://github.com/user` | Link cá nhân |
| avatarUrl | string | Không | `https://res.cloudinary.com/.../avatar.jpg` | Avatar URL (dùng khi có sẵn) |
| avatarFile | file | Không | `avatar.jpg` | Upload file ảnh mới (ưu tiên hơn avatarUrl) |

## Response (200)

```json
{
  "data": null,
  "message": "Profile Updated Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-09T12:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 404 | User Not Found | User không tồn tại | Hiển thị thông báo lỗi |
| 400 | Student Id Cannot Be Changed Once Set | StudentId đã được set trước đó | Báo user ko thể đổi |
