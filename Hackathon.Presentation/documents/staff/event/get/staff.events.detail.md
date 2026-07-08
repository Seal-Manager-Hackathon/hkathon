# GET /api/v1/staff/events/{eventId}

> Staff đã đăng nhập lấy thông tin chi tiết của 1 event mà họ được phân công.

## Nghiệp vụ

API này cho phép **Staff** xem chi tiết của 1 event cụ thể mà họ đã được gán vào (qua bảng `AssignEvents`).

- Staff chỉ xem được event mà họ có bản ghi trong `AssignEvents` với `UserId` trùng với token.
- Event phải không bị disable (`IsDisable = false`).

## Phân quyền
- ✅ Staff (RoleEnum = Staff)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "name": "Hackathon 2026",
    "description": "Mô tả event chi tiết...",
    "status": "Published",
    "numberRound": 3,
    "season": "Summer",
    "startTime": "2026-07-01T00:00:00Z",
    "endTime": "2026-07-15T00:00:00Z",
    "registerLimitTime": "2026-06-30T23:59:59Z",
    "limitTeam": 50,
    "minMember": 2,
    "maxMember": 5,
    "eventRoleId": "guid",
    "eventRoleName": "Judge",
    "createdAt": "2026-06-01T12:00:00Z",
    "updatedAt": "2026-06-10T12:00:00Z"
  },
  "message": "Events Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `id` | ID của event |
| `name` | Tên event |
| `description` | Mô tả event |
| `status` | Trạng thái: Draft, Published, Closed |
| `numberRound` | Số vòng thi |
| `season` | Mùa: Spring, Summer, Fall, Winter |
| `startTime` | Thời gian bắt đầu event |
| `endTime` | Thời gian kết thúc event |
| `registerLimitTime` | Hạn cuối đăng ký tham gia event |
| `limitTeam` | Số team tối đa được đăng ký |
| `minMember` | Số thành viên tối thiểu mỗi team |
| `maxMember` | Số thành viên tối đa mỗi team |
| `eventRoleId` | ID của role staff trong event này |
| `eventRoleName` | Tên role: Mentor, Judge, Staff |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không phải Staff |
| 404 | Event Not Found or You Are Not Assigned to This Event | Event không tồn tại, bị disable hoặc staff không được gán vào event này |
