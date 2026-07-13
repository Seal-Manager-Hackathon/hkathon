# PATCH /api/v1/user/me

> Cập nhật thông tin profile của người dùng hiện tại.

## Nghiệp vụ
- Cập nhật các field: firstName, lastName, phoneNumber, bio, address, dateOfBirth, studentId, imgUrl, linkUrl
- Chỉ cập nhật field có giá trị (nullable)
- Bất kỳ role nào cũng dùng được

## Phân quyền
- ✅ Đăng nhập (Mọi role)

## Request

### Body (JSON)

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| firstName | string | Không | `Nguyễn` | Tên |
| lastName | string | Không | `Văn B` | Họ |
| phoneNumber | string | Không | `0909123456` | Số điện thoại |
| bio | string | Không | `Sinh viên năm 3` | Giới thiệu |
| address | string | Không | `Hà Nội` | Địa chỉ |
| dateOfBirth | datetime | Không | `2000-01-15T00:00:00Z` | Ngày sinh |
| studentId | string | Không | `20210001` | Mã sinh viên |
| imgUrl | string | Không | `https://example.com/img.jpg` | Ảnh đại diện (URL) |
| linkUrl | string | Không | `https://github.com/user` | Link cá nhân |
| avatarUrl | string | Không | `https://res.cloudinary.com/.../avatar.jpg` | Avatar (URL) |

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
