# GET /api/v1/student/assign/events/{eventId}/assigned

> Student lấy danh sách user đã được phân công trong 1 event (Staff, Judge, Mentor), có phân trang và filter.

**Controller:** [StudentAssignController.cs](Controllers/Student/StudentAssignController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/events/{eventId}/assigned)

## Nghiệp vụ

Student muốn xem danh sách những người được phân công vào một event để biết ai đang hỗ trợ event đó.

- Giống Admin API nhưng **chỉ lấy user có IsDisable = false** (ko lấy bản ghi đã bị xóa mềm).
- Các track được gán cũng chỉ lấy track có IsDisable = false.
- Hỗ trợ tìm kiếm theo **email** hoặc **tên** (firstName + lastName).
- Lọc theo **EventRole**: `Mentor`, `Judge`, `Staff`.
- Kết quả sắp xếp theo tên (firstName, lastName).

## Phân quyền
- ✅ Student

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
            "eventId": "guid",
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

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid EventRole | EventRole ko hợp lệ | Validation form |
| 400 | Page Index/Page Size invalid | pageIndex/pageSize ko hợp lệ | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
