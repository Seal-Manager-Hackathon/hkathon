# GET /api/v1/staff/events/recent

> Staff lấy danh sách 10 event mới nhất (dashboard).

**Controller:** [StaffEventController.cs](Controllers/Staff/StaffEventController.cs)

## Nghiệp vụ

- Staff xem 10 event được tạo gần đây nhất trên toàn hệ thống.
- Không filter theo event assign — dashboard overview.
- Giống hệt Admin `GET /api/v1/admin/events/recent`, khác auth là Staff.

## Phân quyền
- ✅ Staff (RoleEnum.Staff)

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "Cuộc thi lập trình",
        "status": "Published",
        "startTime": "2026-07-15T00:00:00Z",
        "endTime": "2026-07-20T00:00:00Z",
        "isDisable": false,
        "createdAt": "2026-07-11T00:00:00Z",
        "updatedAt": "2026-07-11T00:00:00Z"
      }
    ]
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission | Không phải Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/recent)
