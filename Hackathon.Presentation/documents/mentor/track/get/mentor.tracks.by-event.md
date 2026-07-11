# GET /api/v1/mentor/events/{eventId}/tracks

> Mentor lấy danh sách các track được phân công trong 1 event.
> **Controller:** `MentorNotificationController` — `GET /api/v1/mentor/events/{eventId}/tracks`

## Nghiệp vụ

- Mentor muốn xem các track nào họ được phân công (assign) trong 1 event cụ thể.
- **Chỉ lấy track có IsDisable = false** và **track đó phải được assign cho mentor** (qua bảng AssignTracks).
- Mentor phải có AssignEvent trong event đó trước.
- Response format giống hệt Admin `GET /api/v1/admin/events/{eventId}/tracks` (dùng `TrackItem`), chỉ thêm 2 field: `eventRoleId`, `eventRoleName`.
- Hỗ trợ phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Mentor (RoleEnum = Lecturer, có AssignEvent trong event)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ |
|-----------|------|----------|-------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "tracks": [
      {
        "id": "guid",
        "eventId": "guid",
        "title": "AI - Trí tuệ nhân tạo",
        "description": "Các giải pháp ứng dụng AI vào đời sống",
        "maxTeam": 2,
        "isDisable": false,
        "eventRoleId": "guid",
        "eventRoleName": "Mentor",
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
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
  "traceId": "...",
  "timestampUtc": "2026-07-11T00:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Event Not Found or You Are Not Assigned to This Event | eventId ko tồn tại hoặc mentor chưa được assign |

> **Ref:** API gốc cho Mentor.
