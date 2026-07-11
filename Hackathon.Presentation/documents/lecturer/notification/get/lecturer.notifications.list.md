# GET /api/v1/lecturer/notifications

> Lecturer lấy danh sách thông báo dành cho họ (Personal) và thông báo hệ thống (System).

**Controller:** [LecturerNotificationController.cs](Controllers/Lecturer/LecturerNotificationController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/notifications`

- Giống Admin `GET /api/v1/admin/notifications` về request/response.
- **Khác Admin về logic filter:**
  - **Personal (person):** Chỉ lấy notification có `UserId == currentUserId` (của riêng Lecturer đó).
  - **System:** Chỉ lấy notification có `TargetType == System`.
  - Mặc định (không truyền TargetType): lấy cả Personal và System.
  - **Luôn filter `IsDisable = false`** — dù có truyền `IsDisable` trong request cũng ko override được, chỉ lấy notification đang hoạt động.
- Hỗ trợ tìm kiếm theo Title, lọc TargetType, FromDate/ToDate.
- Sắp xếp theo CreatedAt giảm dần.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Title | string | No | - | Tìm kiếm theo tiêu đề |
| TargetType | string | No | - | Enum: Personal, Team, System |
| FromDate | datetime | No | - | Lọc từ ngày tạo |
| ToDate | datetime | No | - | Lọc đến ngày tạo |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "userId": "guid",
        "teamId": null,
        "title": "Kết quả vòng 1",
        "status": "Unread",
        "description": "Điểm của bạn...",
        "targetType": "Personal",
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:30:00Z"
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
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
| 400 | Invalid TargetType. Must be: Personal, Team, System | TargetType sai |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/notifications)
