# GET /api/v1/lecturer/events/{eventId}/my-assignment

> Lecturer lấy thông tin phân công của chính mình trong event (role, tracks được gán).

**Controller:** [LecturerAssignController.cs](Controllers/Lecturer/LecturerAssignController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/my-assignment`

- Lecturer đang đăng nhập xem thông tin assigned của mình trong một event.
- Trả về: EventRole hiện tại (Judge/Mentor) và danh sách tracks được gán.
- Chỉ lấy track còn active (`IsDisable = false` và `Track.IsDisable = false`).

## Phân quyền
- ✅ Lecturer (phải được assign vào event)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

## Response (200)
```json
{
  "data": {
    "assignEventId": "guid",
    "eventId": "guid",
    "eventRole": "Judge",
    "tracks": [
      {
        "assignTrackId": "guid",
        "trackId": "guid",
        "title": "AI - Chatbot",
        "isDisable": false
      }
    ]
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 404 | Event Not Found or You Are Not Assigned to This Event | Event không tồn tại hoặc chưa được assign |

> **Ref:** Tương tự Staff get assigned info
