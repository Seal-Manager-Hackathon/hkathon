# GET /api/v1/staff/events/{eventId}/assigned/{assignEventId}

> Staff xem chi tiet mot ban ghi phan cong trong event.

## Nghiep vu
- Staff phai duoc phan cong vao event tuong ung.
- Tra ve thong tin chi tiet assign: user, vai tro, cac track duoc gan, thoi gian tao/cap nhat.
- Neu assign bi disable (`IsDisable = true`), API tra ve 404.
- Chi lay track co `IsDisable = false`.

## Phan quyen
- ✅ Staff (phai duoc phan cong vao event tuong ung)

## Request

### Route Parameters
| Parameter | Type | Bat buoc | Vi du | Ghi chu |
|-----------|------|----------|-------|---------|
| eventId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua event |
| assignEventId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua ban ghi assign trong bang `AssignEvents` |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "firstName": "Nguyen",
    "lastName": "Van A",
    "avatarUrl": "https://example.com/avatar.jpg",
    "eventRole": "Judge",
    "tracks": [
      {
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "Tri tue nhan tao",
        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
      }
    ],
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Loi
| Status | message | Khi nao | FE xu ly |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token het han/thieu | Chuyen ve trang login |
| 403 | You do not have permission to perform this action | Khong phai Staff hoac khong duoc phan cong vao event | Hien thi thong bao khong co quyen |
| 404 | Resource Not Found | AssignEventId khong ton tai hoac da bi disable | Hien thi thong bao khong tim thay |
