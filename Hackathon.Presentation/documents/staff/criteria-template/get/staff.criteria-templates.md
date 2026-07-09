# GET /api/v1/staff/events/{eventId}/rounds/{roundId}/criteria-templates

> Staff xem danh sách criteria template của round.

## Nghiep vu
- Staff phai duoc phan cong vao event tuong ung.
- Chi tra ve template co `IsDisable = false`.
- Entity `CriteriaTemplate` dung field `Title` anh xa sang `name` trong response.

## Phan quyen
- ✅ Staff (phai duoc phan cong vao event tuong ung)

## Request

### Route Parameters
| Parameter | Type | Bat buoc | Vi du | Ghi chu |
|-----------|------|----------|-------|---------|
| eventId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua event |
| roundId | Guid | Co | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID cua round |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Tieu chi cham diem vong loai",
        "description": "Danh gia y tuong va kha thi",
        "isDisable": false,
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ]
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
| 404 | Resource Not Found | RoundId khong ton tai | Hien thi thong bao khong tim thay |
