# GET /api/v1/staff/events

> Staff đã đăng nhập lấy danh sách các event mà họ được phân công, có filter và phân trang.

## Nghiệp vụ

API này cho phép **Staff** xem danh sách tất cả các event mà họ được gán vào (qua bảng `AssignEvents`).

- **Mặc định loại trừ event có status `Draft`**: Staff chỉ thấy event đã Published hoặc Closed.
- Hỗ trợ tìm kiếm theo **từ khóa** (tên event), lọc theo **status** (Published/Closed), và khoảng **thời gian** tạo event.
- Kết quả sắp xếp theo ngày tạo event mới nhất trước.

## Phân quyền
- ✅ Staff (RoleEnum = Staff)

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm kiếm theo tên event (không phân biệt hoa thường) |
| status | string | No | - | Lọc theo trạng thái: `Published`, `Closed` |
| fromDate | DateTimeOffset | No | - | Lọc event tạo từ ngày này trở đi |
| toDate | DateTimeOffset | No | - | Lọc event tạo đến ngày này |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "Mô tả event...",
        "status": "Published",
        "numberRound": 3,
        "season": "Summer",
        "startTime": "2026-07-01T00:00:00Z",
        "endTime": "2026-07-15T00:00:00Z",
        "eventRoleId": "guid",
        "eventRoleName": "Judge",
        "createdAt": "2026-06-01T12:00:00Z",
        "updatedAt": "2026-06-10T12:00:00Z"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
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
| `status` | Trạng thái: Draft, Published, Closed (**Draft bị loại trừ**) |
| `numberRound` | Số vòng thi |
| `season` | Mùa: Spring, Summer, Fall, Winter |
| `startTime` | Thời gian bắt đầu event |
| `endTime` | Thời gian kết thúc event |
| `eventRoleId` | ID của role staff trong event này |
| `eventRoleName` | Tên role: Mentor, Judge, Staff |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không phải Staff |
