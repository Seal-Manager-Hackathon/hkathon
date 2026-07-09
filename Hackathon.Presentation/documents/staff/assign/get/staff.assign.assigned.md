# GET /api/v1/staff/events/{eventId}/assigned

> Staff lấy danh sách user đã được phân công trong event (chỉ các bản ghi đang active).

## Nghiệp vụ
- Staff phải được phân công vào event tương ứng.
- Trả về danh sách user đã được assign vào event, **chỉ các bản ghi đang active** (`IsDisable = false`).
- Mỗi user có thể được gắn vào nhiều track — chỉ lấy track đang active (`IsDisable = false`).
- Hỗ trợ lọc theo keyword, EventRole, Role (User role), TrackId.

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của event |

### Query Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| Keyword | string | Không | nguyen van a | Tìm kiếm theo email hoặc fullname |
| EventRole | string | Không | Judge | Lọc theo event role: `Mentor`, `Judge`, `Staff` |
| Role | string | Không | Lecturer | Lọc theo user role: `Admin`, `Staff`, `Student`, `Lecturer` |
| TrackId | Guid | Không | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | Lọc theo track được phân công |
| PageIndex | int | Không (mặc định 1) | 1 | Trang hiện tại |
| PageSize | int | Không (mặc định 10) | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "assignEventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "user@example.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": "https://example.com/avatar.jpg",
        "eventRole": "Judge",
        "assignTracks": [
          {
            "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "title": "Tri tue nhan tao",
            "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
          }
        ]
      }
    ],
    "totalCount": 5,
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
| 400 | Invalid EventRole | EventRole không hợp lệ | Hiển thị thông báo lỗi |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ | Hiển thị thông báo lỗi |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Chuyển về trang login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Hiển thị thông báo Không có quyền |
