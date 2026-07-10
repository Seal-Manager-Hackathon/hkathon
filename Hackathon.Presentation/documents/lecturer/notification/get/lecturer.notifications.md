# GET /api/v1/lecturer/notifications

> Lecturer lấy danh sách thông báo của bản thân (System + Personal dành cho họ), có filter và phân trang.

## Nghiệp vụ
- Trả về các thông báo mà lecturer được nhận: 
  - **System** (`TargetType = System`) — thông báo hệ thống, gửi cho tất cả user
  - **Personal** (`TargetType = Personal`, `UserId = userId của lecturer`) — thông báo riêng
- Mặc định không lấy thông báo đã bị disable.
- Hỗ trợ tìm kiếm theo tiêu đề, lọc theo TargetType, Status (Read/Unread), khoảng thời gian.
- Sắp xếp mới nhất trước.

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer)

## Request

### Query Parameters
| Parameter | Type | Bắt buộc | Mặc định | Ghi chú |
|-----------|------|----------|---------|---------|
| Keyword | string | Không | - | Tìm kiếm theo tiêu đề |
| TargetType | string | Không | - | Lọc: `Personal`, `System`, `Team` |
| Status | string | Không | - | Lọc: `Unread`, `Read` |
| FromDate | DateTimeOffset | Không | - | Từ ngày (CreatedAt) |
| ToDate | DateTimeOffset | Không | - | Đến ngày (CreatedAt) |
| PageIndex | int | Không | 1 | Trang |
| PageSize | int | Không | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": null,
        "teamId": null,
        "title": "Hackathon AI 2026 đã kết thúc",
        "status": "Unread",
        "description": "Cảm ơn bạn đã tham gia...",
        "targetType": "System",
        "isDisable": false,
        "createdAt": "2026-07-10T00:00:00Z",
        "updatedAt": "2026-07-10T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid TargetType/Status | Sai enum |
| 400 | PageIndex/PageSize invalid | Pagination sai |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Lecturer |
