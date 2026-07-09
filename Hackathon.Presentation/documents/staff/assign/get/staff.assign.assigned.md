# GET /api/v1/staff/events/{eventId}/assigned

> Staff lay danh sach user da duoc phan cong trong event.

## Nghiep vu
- Staff phai duoc phan cong vao event tuong ung.
- Tra ve danh sach user da duoc assign vao event, kem thong tin tracks ho duoc phan cong.
- Moi user co the duoc gan vao nhieu track thong qua bang `AssignTracks`.
- Ho tro loc theo keyword, EventRole, Role (User role), TrackId.

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
| EventRole | string | Khong | Judge | Loc theo event role: `Mentor`, `Judge`, `Staff` |
| Role | string | Khong | Lecturer | Loc theo user role: `Admin`, `Staff`, `Student`, `Lecturer` |
| TrackId | Guid | Khong | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | Loc theo track duoc phan cong |
| PageIndex | int | Khong (mac dinh 1) | 1 | Trang hien tai |
| PageSize | int | Khong (mac dinh 10) | 10 | So luong item moi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "assignEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "user@example.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": "https://example.com/avatar.jpg",
        "eventRole": "Judge",
        "assignTracks": [
          {
            "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "title": "Tri tue nhan tao",
            "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
          }
        ]
      }
    ],
    "totalCount": 5,
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
| 400 | Invalid EventRole | EventRole khong hop le | Hien thi thong bao loi |
| 400 | PageIndex/PageSize invalid | PageIndex hoac PageSize khong hop le | Hien thi thong bao loi |
| 401 | Invalid Or Expired Token | Token het han/thieu | Chuyen ve trang login |
| 403 | You do not have permission to perform this action | Khong phai Staff hoac khong duoc phan cong vao event | Hien thi thong bao khong co quyen |
