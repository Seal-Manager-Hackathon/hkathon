# GET /api/v1/staff/events/{eventId}/rounds

> Staff đã đăng nhập lấy danh sách các round của event mà họ được phân công, có filter và phân trang.

## Nghiệp vụ

Staff muốn xem danh sách các vòng thi (round) của một event mà họ được phân công. Đầu tiên, hệ thống kiểm tra xem staff có được gán vào event này không (qua bảng `AssignEvents`). Nếu có, trả về danh sách round của event đó.

- **Chỉ lấy round chưa bị disable**: `IsDisable = false`. Staff không thấy các round đã bị vô hiệu hóa.
- Hỗ trợ tìm kiếm theo **từ khóa** (tên round) và lọc theo **RoundNo**.
- Kết quả sắp xếp theo `RoundNo` tăng dần.

## Phân quyền
- ✅ Staff (RoleEnum = Staff)
- Staff phải được phân công trong event này (có bản ghi trong `AssignEvents`)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm kiếm theo tên round (không phân biệt hoa thường) |
| roundNo | int | No | - | Lọc round có số thứ tự cụ thể |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "rounds": [
      {
        "id": "guid",
        "eventId": "guid",
        "name": "Vòng 1: Ý tưởng",
        "description": "Vòng nộp ý tưởng ban đầu",
        "roundNo": 1,
        "startTime": "2026-07-01T00:00:00Z",
        "endTime": "2026-07-05T00:00:00Z",
        "startSubmission": "2026-07-01T00:00:00Z",
        "endSubmission": "2026-07-04T23:59:59Z",
        "limitTeam": 30,
        "createdAt": "2026-06-01T12:00:00Z",
        "updatedAt": "2026-06-10T12:00:00Z"
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Rounds Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `id` | ID của round |
| `eventId` | ID của event chứa round này |
| `name` | Tên round |
| `description` | Mô tả round |
| `roundNo` | Số thứ tự của round trong event |
| `startTime` | Thời gian bắt đầu round |
| `endTime` | Thời gian kết thúc round |
| `startSubmission` | Thời gian bắt đầu nhận bài nộp |
| `endSubmission` | Hạn chót nộp bài |
| `limitTeam` | Số team tối đa được vào round này |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission to perform this action | User không phải Staff |
| 403 | You Are Not Assigned to This Event | Staff không được phân công vào event này |
