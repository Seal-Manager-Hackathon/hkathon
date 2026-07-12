# GET /api/v1/student/events/{eventId}/rounds

> Student lấy danh sách round của 1 event (chỉ round ko bị disable).

**Controller:** [StudentRoundController.cs](Controllers/Student/StudentRoundController.cs)

## Nghiệp vụ
- Lấy danh sách round của 1 event.
- **Tự động filter** chỉ lấy round có `IsDisable = false`.
- Hỗ trợ tìm kiếm theo tên (contains, ko phân biệt hoa thường), lọc theo roundNo.
- 404 nếu eventId ko tồn tại.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| eventId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |
| Keyword | string | ❌ (query) | `Vòng 1` |
| RoundNo | int | ❌ (query) | `1` |
| PageIndex | int | ❌ (query) | `1` |
| PageSize | int | ❌ (query) | `10` |

## Response (200)
```json
{
  "data": {
    "rounds": [
      {
        "id": "guid",
        "eventId": "guid",
        "name": "Vòng 1",
        "description": "...",
        "roundNo": 1,
        "startTime": "2026-08-01T00:00:00Z",
        "endTime": "2026-08-03T00:00:00Z",
        "startSubmission": "2026-08-03T00:00:00Z",
        "endSubmission": "2026-08-05T00:00:00Z",
        "limitTeam": 20,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Page Index Must Be Greater Than 0 | pageIndex < 1 | Báo "Trang không hợp lệ" |
| 400 | Page Size Must Be Between 1 And 100 | pageSize < 1 hoặc > 100 | Báo "Kích thước trang không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Event Not Found | eventId ko tồn tại | Báo "Không tìm thấy sự kiện" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/rounds)
