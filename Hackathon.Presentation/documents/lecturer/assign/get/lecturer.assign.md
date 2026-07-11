# GET /api/v1/lecturer/events/{eventId}/assign

> Lecturer lấy danh sách users được phân công vào event (có filter, phân trang).

**Controller:** [LecturerAssignController.cs](Controllers/Lecturer/LecturerAssignController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/assign`

- Giống hệt Admin `GET /api/v1/admin/assign/events/{eventId}/assigned`, khác auth là Lecturer.
- Lecturer phải được assign vào event này mới xem được.
- Trả về danh sách user được phân công, kèm thông tin tracks được gán.
- Mặc định filter `IsDisable = false` (chỉ lấy assign còn active).
- Hỗ trợ tìm kiếm theo email/họ tên và lọc theo EventRole.

## Phân quyền
- ✅ Lecturer (phải được assign vào event)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo email hoặc fullname |
| EventRole | string | No | - | Lọc theo role: `Mentor`, `Judge`, `Staff` |
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
        "email": "lecturer@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "avatarUrl": null,
        "eventRole": "Judge",
        "isDisable": false,
        "assignTracks": [
          {
            "trackId": "guid",
            "title": "AI - Chatbot",
            "eventId": "guid",
            "isDisable": false
          }
        ]
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer hoặc không được assign vào event |
| 400 | Invalid EventRole | EventRole không hợp lệ |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ |

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/events/{eventId}/assigned)
