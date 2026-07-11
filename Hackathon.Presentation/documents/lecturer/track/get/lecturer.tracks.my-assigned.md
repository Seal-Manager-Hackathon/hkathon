# GET /api/v1/lecturer/events/{eventId}/my-tracks

> Lecturer xem danh sách track được phân công trong 1 event (chỉ track còn hoạt động), kèm event role, phân trang.

**Controller:** [LecturerTrackController.cs](Controllers/Lecturer/LecturerTrackController.cs)

## Nghiệp vụ

- Lecturer được phân công vào event (qua AssignEvents) — API này trả ra các track họ được assign.
- **Tự động lọc** chỉ lấy track `IsDisable = false` (cả assign track và track).
- Response format giống Admin track item, **thêm** `eventRoleId` + `eventRoleName`.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer — phải được assign vào event

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

## Response (200)
```json
{
  "data": {
    "tracks": [
      {
        "id": "guid",
        "eventId": "guid",
        "title": "AI - Trí tuệ nhân tạo",
        "description": "Các giải pháp ứng dụng AI",
        "maxTeam": 5,
        "isDisable": false,
        "eventRoleId": "guid",
        "eventRoleName": "Mentor",
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Event Not Found or You Are Not Assigned | eventId ko tồn tại hoặc ko dc phân công |
