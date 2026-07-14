# GET /api/v1/admin/notifications/my

> Admin lấy danh sách thông báo của riêng họ (Personal) và thông báo hệ thống (System), có filter và phân trang.

## Nghiệp vụ

- **Personal:** Chỉ lấy notification có `UserId == currentUserId` (của riêng admin đó).
- **System:** Chỉ lấy notification có `TargetType == System`.
- Mặc định lấy cả Personal và System.
- Hỗ trợ lọc theo Keyword, TargetType, Status (Unread/Read), FromDate/ToDate.
- Sắp xếp theo CreatedAt giảm dần.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Admin

## Request

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Tìm theo title |
| TargetType | string | ❌ | `System` | ⚠️ Enum: Personal, System |
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
        "isDisable": false,
        "createdAt": "2026-07-13T10:00:00Z",
        "updatedAt": "2026-07-13T10:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid TargetType / Status | TargetType hoặc Status sai |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |

> **Ref:** [Lecturer API tương ứng](/api/v1/lecturer/notifications)
