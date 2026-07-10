# GET /api/v1/judge/events/{eventId}/tracks

> Judge đã đăng nhập lấy danh sách các track mà họ được phân công chấm trong 1 event.

**Controller:** `JudgeController` — `GET /api/v1/judge/events/{eventId}/tracks`

## Nghiệp vụ

- Judge được Admin/Staff assign vào event với EventRole = Judge, và assign vào 1 hoặc nhiều track.
- API này trả về danh sách track mà judge hiện tại được assign chấm điểm trong event.
- **Chỉ lấy track có `IsDisable = false`** — cả AssignTrack và Track đều phải active.

## Phân quyền
- ✅ Judge (RoleEnum.Lecturer + EventRoleEnum.Judge) — phải được assign vào event

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

## Response (200)
```json
{
  "data": [
    {
      "assignTrackId": "guid",
      "trackId": "guid",
      "trackTitle": "Tri tue nhan tao",
      "trackDescription": "...",
      "eventId": "guid",
      "eventName": "Hackathon 2026"
    }
  ],
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| assignTrackId | ID của bản ghi AssignTrack (dùng để chấm điểm) |
| trackId | ID của track |
| trackTitle | Tên track |
| trackDescription | Mô tả track (có thể null) |
| eventId | ID của event |
| eventName | Tên event |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Event | User ko phải Judge |
| 404 | Event Not Found or You Are Not Assigned to This Event | eventId ko tồn tại hoặc ko được assign |
