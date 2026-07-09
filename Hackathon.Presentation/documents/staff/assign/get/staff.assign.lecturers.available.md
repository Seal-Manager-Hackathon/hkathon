# GET /api/v1/staff/events/{eventId}/lecturers/available

> Staff lay danh sach Lecturer chua duoc phan cong vao event.

## Nghiep vu
- Staff phai duoc phan cong vao event tuong ung.
- Chi tra ve user co role = `Lecturer`.
- Chi lay Lecturer chua duoc assign vao event nay.
- Staff chi co the assign Lecturer voi EventRole la `Judge` hoac `Mentor`.

## Phan quyen
- ✅ Staff (phai duoc phan cong vao event tuong ung)

## Request

### Route Parameters
| Parameter | Type | Bat buoc | Vi du | Ghi chu |
|-----------|------|----------|-------|---------|
| eventId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua event |

### Query Parameters
| Parameter | Type | Bat buoc | Vi du | Ghi chu |
|-----------|------|----------|-------|---------|
| Keyword | string | Khong | nguyen van a | Tim kiem theo email hoac fullname |
| PageIndex | int | Khong (mac dinh 1) | 1 | Trang hien tai |
| PageSize | int | Khong (mac dinh 10) | 10 | So luong item moi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "lecturer@example.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": "https://example.com/avatar.jpg",
        "college": "Dai hoc Bach Khoa",
        "phoneNumber": "0909123456"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
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
