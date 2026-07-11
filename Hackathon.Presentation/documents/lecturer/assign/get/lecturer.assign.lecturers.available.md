# GET /api/v1/lecturer/events/{eventId}/lecturers/available

> Lecturer lấy danh sách Lecturer chưa được phân công vào event (không bị ban/disable).

**Controller:** [LecturerAssignController.cs](Controllers/Lecturer/LecturerAssignController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/{eventId}/lecturers/available`

- Giống hệt Admin `GET /api/v1/admin/assign/events/{eventId}/lecturers/available`, khác auth là Lecturer.
- Chỉ trả về user có role = `Lecturer`.
- Tự động loại bỏ user đã bị **disable** (`IsDisable = true`) hoặc đã bị **ban** (`BanReason != null`).
- Chỉ lấy Lecturer chưa được assign vào event này (tránh duplicate).
- Hỗ trợ tìm kiếm theo email hoặc họ tên.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo email hoặc fullname |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "email": "lecturer@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "avatarUrl": "https://robohash.org/...",
        "college": "FPT University",
        "phoneNumber": "0123456789"
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
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ |

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/events/{eventId}/lecturers/available)
