# GET /api/v1/staff/teams/count

> Staff đếm số lượng teams. Có thể lọc theo trạng thái isDisable.

**Controller:** [StaffTeamController.cs](Controllers/Staff/StaffTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/staff/teams/count`

- Giống hệt Admin `GET /api/v1/admin/teams/count`, khác auth là Staff.

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| IsDisable | bool | ❌ | false | true/false. Không truyền = lấy tất cả |

## Response (200)
```json
{
  "data": { "total": 15 },
  "message": "Team Count Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/count)
