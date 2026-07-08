# POST /api/v1/user/avatar

> Người dùng đã đăng nhập upload ảnh đại diện (avatar) mới.

## Nghiệp vụ

Người dùng muốn thay đổi ảnh đại diện của mình. Họ upload file ảnh lên hệ thống, ảnh sẽ được lưu trữ trên Cloudinary và URL ảnh mới được gắn vào trường `AvatarUrl` của user.

Luồng xử lý:
1. Kiểm tra user đã đăng nhập (bất kỳ role nào: Student, Lecturer, Staff, Admin đều dùng được).
2. Upload file ảnh lên Cloudinary qua `IMediaService.UploadImageAsync` (folder `avatars`).
3. Cập nhật `AvatarUrl` trong DB thành URL mới trả về từ Cloudinary.
4. Trả về `avatarUrl` (URL ảnh đã upload).

## Phân quyền
- ✅ Đăng nhập (Mọi role: Student, Lecturer, Staff, Admin)

## Request

### Form Data (multipart/form-data)
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| file | file (IFormFile) | Yes | File ảnh (png, jpg, jpeg, webp...) |

Lưu ý: gửi dưới dạng `multipart/form-data`, key là `file`.

## Response (200)
```json
{
  "data": {
    "avatarUrl": "https://res.cloudinary.com/xxx/image/upload/v123/avatars/abc.jpg"
  },
  "message": "Avatar Updated Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `avatarUrl` | URL đầy đủ của ảnh đại diện mới trên Cloudinary |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | File Is Required | Không gửi file hoặc file rỗng |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 404 | User Not Found | User không tồn tại trong hệ thống |
