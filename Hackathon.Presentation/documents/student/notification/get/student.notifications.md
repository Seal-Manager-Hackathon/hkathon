# GET /api/v1/student/notifications

> Student lấy danh sách notification — bao gồm Personal (của riêng user), Team (của team đang tham gia), và System (toàn hệ thống).

**Controller:** [StudentNotificationController.cs](Controllers/Student/StudentNotificationController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/student/notifications`

- **Personal:** notification có `UserId = user hiện tại`, target type = Personal.
- **Team:** notification có `TeamId` thuộc các team user đang tham gia. Chỉ lấy team **ko bị disable** (`Team.IsDisable = false`) và **team detail active** (`TeamDetail.Status = Active`, `TeamDetail.IsDisable = false`).
- **System:** tất cả notification target type = System.
- Mặc định **chỉ lấy notification ko bị disable** (`Notifications.IsDisable = false`).
- Hỗ trợ lọc theo `targetType` (Personal / Team / System), `status` (Unread / Read), khoảng thời gian, keyword (tìm theo title).
- Sắp xếp theo `CreatedAt` giảm dần (mới nhất lên đầu).
- Hỗ trợ phân trang.

## Phân quyền
- ✅ Student

## Request

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Tìm theo title |
| TargetType | string | ❌ | `Team` | ⚠️ Enum: Personal, Team, System |
| Status | string | ❌ | `Unread` | ⚠️ Enum: Unread, Read |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | Lọc từ ngày |
| ToDate | datetime | ❌ | `2026-07-13T23:59:59Z` | Lọc đến ngày |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)

```json
{
  "data": {
    "notifications": [
      {
        "id": "guid",
        "userId": "guid",
        "teamId": null,
        "title": "Thông báo mới",
        "status": "Unread",
        "description": "Nội dung thông báo",
        "targetType": "Personal",
        "createdAt": "2026-07-13T10:00:00Z",
        "updatedAt": "2026-07-13T10:00:00Z"
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
  "timestampUtc": "2026-07-13T12:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid TargetType. Must be: Personal, Team, System | TargetType sai |
| 400 | Invalid Status. Must be: Unread, Read | Status sai |
| 400 | Page Index / Page Size | Pagination sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Student |

> **Ref:** API này không có Admin tương ứng — là API riêng cho Student.
