# GET /api/v1/judge/events/{eventId}/tracks

> Judge lấy danh sách track được phân công trong 1 event (giống Mentor).

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Judge lấy các track được assign trong 1 event cụ thể.
- Check EventRole = Judge.
- Chỉ lấy track không bị disable.

## Phân quyền
- ✅ Judge — phải được assign vào event với EventRole = Judge

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
| 403 | You Are Not Assigned as Judge for This Event | Ko phải Judge |
| 404 | Event Not Found or You Are Not Assigned | eventId ko tồn tại |
