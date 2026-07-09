# GET api/v1/admin/assign/events/{eventId}/assigned

> Admin lấy danh sách user đã được phân công trong 1 event (Staff, Judge, Mentor), có phân trang và filter.

## Nghiệp vụ

Admin muốn xem danh sách tất cả user được phân công vào một event cụ thể. Kết quả trả về bao gồm thông tin user, event role (Staff/Judge/Mentor), các track được gán, và trạng thái disable của từng bản ghi phân công.

- Hỗ trợ tìm kiếm theo **email** hoặc **tên** (firstName + lastName).
- Lọc theo **EventRole**: `Mentor`, `Judge`, `Staff`.
- Trả về **IsDisable** cho cả bản ghi phân công (AssignEvents) và từng track được gán (AssignTracks), giúp admin biết bản ghi nào đang active hoặc đã bị xóa mềm.
- Kết quả sắp xếp theo tên (firstName, lastName).

Liên quan: [xóa phân công event](../post/admin.assign.event-remove.md), [restore phân công event](../post/admin.assign.event-restore.md), [xóa track](../post/admin.assign.tracks.remove.md), [restore track](../post/admin.assign.tracks.restore.md).

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo email hoặc fullname |
| EventRole | string | No | - | Lọc theo event role: `Mentor`, `Judge`, `Staff` |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "assignEventId": "guid",
        "userId": "guid",
        "email": "staff@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "avatarUrl": "https://robohash.org/...",
        "eventRole": "Mentor",
        "isDisable": false,
        "assignTracks": [
          {
            "trackId": "guid",
            "title": "AI Track",
            "isDisable": false
          }
        ]
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `assignEventId` | ID của bản ghi phân công (AssignEvents) |
| `userId` | ID của user được phân công |
| `email` | Email của user |
| `firstName` / `lastName` | Họ và tên của user |
| `avatarUrl` | URL ảnh đại diện |
| `eventRole` | Vai trò trong event: Staff, Judge, Mentor |
| **`isDisable`** | Trạng thái của bản ghi phân công: `false` = đang active, `true` = đã bị xóa mềm |
| `assignTracks` | Danh sách track được gán cho user này trong event |
| `assignTracks[].isDisable` | Trạng thái của track được gán: `false` = active, `true` = đã bị xóa mềm |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 400 | Invalid EventRole | EventRole không hợp lệ (không phải Mentor/Judge/Staff) |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ |
