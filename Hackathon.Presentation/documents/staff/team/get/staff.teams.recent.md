# GET /api/v1/staff/teams/recent

> Staff lấy 10 teams được tạo gần nhất.

**Controller:** [StaffTeamController.cs](Controllers/Staff/StaffTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/staff/teams/recent`

- Giống Admin về format response, khác auth là Staff.
- Sắp xếp theo CreatedAt giảm dần, lấy 10 teams mới nhất.

## Phân quyền
- ✅ Staff

## Response (200)
```json
{
  "data": {
    "teams": [
      {
        "id": "guid",
        "name": "FTeam",
        "canEdit": true,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ]
  },
  "message": "Fetched Successfully",
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

> **Ref:** Admin không có API tương ứng
