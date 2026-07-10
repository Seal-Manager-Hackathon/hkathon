# GET /api/v1/admin/teams/{teamId}/events

> Admin lấy danh sách event mà team đã tham gia.

## Nghiệp vụ

Lấy các event mà team đã được approve tham gia. Dựa trên các RegisterTeam có Status=Approved và IsDisable=false của team đó.

## Phân quyền
- ✅ Admin

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số bản ghi mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "eventId": "guid",
        "eventName": "Hackathon AI 2026",
        "status": "Published",
        "createdAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không có role Admin |
