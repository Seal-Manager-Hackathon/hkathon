# GET /api/v1/lecturer/events/{eventId}/register-teams

> Lecturer lấy danh sách register teams trong event, chỉ lấy bản ghi active (IsDisable = false), có filter và phân trang.

## Nghiệp vụ

Lecturer xem danh sách các đội đã đăng ký vào event mà họ tham gia.

- **Chỉ lấy register team có `IsDisable = false`** (không cho lọc bằng IsDisable).
- Có thể lọc theo Status, IsBanned, RoundId, TrackId, TopicId, từ khóa, khoảng thời gian.
- Lecturer phải được phân công vào event.

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer) — phải được assign vào event

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| eventId | Guid | ✅ | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm |
| Status | string | No | - | Lọc: Pending, Approved, Rejected, Banned |
| IsBanned | bool | No | - | Lọc theo trạng thái ban |
| FromDate | datetime | No | - | Từ ngày |
| ToDate | datetime | No | - | Đến ngày |
| RoundId | Guid | No | - | Lọc theo round |
| TrackId | Guid | No | - | Lọc theo track |
| TopicId | Guid | No | - | Lọc theo topic |
| PageIndex | int | No | 1 | Trang |
| PageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "id": "guid",
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team 1",
        "eventId": "guid",
        "eventName": "Hackathon",
        "trackId": "guid",
        "trackName": "AI",
        "topicId": "guid",
        "topicName": "Xử lý ảnh",
        "description": "...",
        "rejectionReason": null,
        "status": "Pending",
        "isBanned": false,
        "isDisable": false,
        "roundId": "guid",
        "roundName": "Vòng 1",
        "roundNo": 1,
        "createdAt": "...",
        "updatedAt": "..."
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Register Teams Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status | Status sai |
| 400 | PageIndex/PageSize invalid | Pagination sai |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned to This Event | Lecturer chưa được assign |
| 403 | You do not have permission | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/register-teams)
