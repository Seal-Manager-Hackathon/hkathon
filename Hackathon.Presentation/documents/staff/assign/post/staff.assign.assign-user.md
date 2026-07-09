# POST /api/v1/staff/events/{eventId}/assign/users

> Staff phan cong Lecturer vao event voi vai tro Judge hoac Mentor.

## Nghiep vu
- Staff phai duoc phan cong vao event tuong ung.
- Chi co the assign user co role = `Lecturer`.
- EventRole hop le: `Judge` hoac `Mentor` — khong the assign role `Staff`.
- Moi Lecturer chi duoc assign vao mot event mot lan (kiem tra duplicate).
- Staff assign dua tren `UserId` cua Lecturer.

## Phan quyen
- ✅ Staff (phai duoc phan cong vao event tuong ung)

## Request

### Route Parameters
| Parameter | Type | Bat buoc | Vi du | Ghi chu |
|-----------|------|----------|-------|---------|
| eventId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua event |

### Body (JSON)
| Field | Type | Bat buoc | Vi du | Ghi chu |
|-------|------|----------|-------|---------|
| userId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua Lecturer can assign |
| eventRole | string | Co | Judge | Vai tro trong event: `Judge` hoac `Mentor` |

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Loi
| Status | message | Khi nao | FE xu ly |
|--------|---------|---------|----------|
| 400 | Can Only Assign Lecturer To Event | User khong phai Lecturer | Hien thi thong bao loi |
| 400 | Staff Cannot Assign Staff Role | EventRole la `Staff` | Hien thi thong bao loi |
| 400 | Invalid EventRole | EventRole khong phai Judge/Mentor | Hien thi thong bao loi |
| 401 | Invalid Or Expired Token | Token het han/thieu | Chuyen ve trang login |
| 403 | You do not have permission to perform this action | Khong phai Staff hoac khong duoc phan cong vao event | Hien thi thong bao khong co quyen |
| 404 | User Not Found | UserId khong ton tai | Hien thi thong bao khong tim thay |
| 404 | Event Role Not Found | EventRole khong ton tai trong he thong | Hien thi thong bao loi |
| 409 | User Is Already Assigned To This Event | Lecturer da duoc assign vao event nay | Hien thi thong bao trung lap |
