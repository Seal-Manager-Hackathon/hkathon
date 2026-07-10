# GET /api/v1/staff/notifications/my

> Staff lấy danh sách thông báo của bản thân (System + Personal dành cho họ), có filter và phân trang.

## Nghiệp vụ
- Trả về các thông báo mà staff được nhận: System + Personal gắn với userId của họ.
- Hỗ trợ tìm kiếm, lọc TargetType, Status (Read/Unread), khoảng thời gian.

## Phân quyền
- ✅ Staff (RoleEnum = Staff)

## Request

### Query Parameters
| Parameter | Type | Bắt buộc | Mặc định | Ghi chú |
|-----------|------|----------|---------|---------|
| Keyword | string | Không | - | Tìm kiếm theo tiêu đề |
| TargetType | string | Không | - | Lọc: `Personal`, `System`, `Team` |
| Status | string | Không | - | Lọc: `Unread`, `Read` |
| FromDate | DateTimeOffset | Không | - | Từ ngày |
| ToDate | DateTimeOffset | Không | - | Đến ngày |
| PageIndex | int | Không | 1 | Trang |
| PageSize | int | Không | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
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
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-10T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid TargetType/Status | Sai enum |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You do not have permission | Không phải Staff |

> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.
