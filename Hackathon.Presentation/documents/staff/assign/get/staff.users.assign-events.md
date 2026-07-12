# GET /api/v1/staff/assign/users/{userId}/assign-events

> Staff lấy danh sách event mà 1 user (Staff hoặc Lecturer khác) được phân công, kèm thông tin event role.

**Controller:** [StaffAssignController.cs](Controllers/Staff/StaffAssignController.cs)

## Nghiệp vụ

Staff muốn xem 1 user Staff hay Lecturer đang được phân công vào những event nào:
- User phải có role **Staff** hoặc **Lecturer**.
- **Chỉ lấy assignment đang active** (`AssignEvents.IsDisable = false`).
- **Event vẫn hiển thị dù bị disable hay không** (`Event.IsDisable = true` vẫn xuất hiện).
- Có thể lọc theo `eventRole` (Staff / Judge / Mentor). Ko truyền = lấy hết.
- Hỗ trợ tìm kiếm theo keyword (tên event).
- Sắp xếp theo `CreatedAt` giảm dần.
- Hỗ trợ phân trang.

> API này tương tự `GET /api/v1/admin/users/{userId}/assign-events` dành cho Admin.

## Phân quyền
- ✅ Staff (phải được assign vào ít nhất 1 event)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| userId | Guid | ID của Staff hoặc Lecturer |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm theo tên event |
| eventRole | string | No | - | ⚠️ Lọc theo event role: Staff, Judge, Mentor |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon AI 2026",
        "description": "Cuộc thi trí tuệ nhân tạo",
        "status": "Published",
        "numberRound": 3,
        "season": "Summer",
        "startTime": "2026-06-01T00:00:00Z",
        "endTime": "2026-07-01T00:00:00Z",
        "eventRoleId": "guid",
        "eventRoleName": "Staff",
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z",
        "isDisable": false
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid EventRole | eventRole sai |
| 400 | User Must Be Staff Or Lecturer | User ko phải Staff/Lecturer |
| 400 | Page Index / Page Size | Pagination sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Staff chưa được assign event nào |
| 404 | User Not Found | userId ko tồn tại |
