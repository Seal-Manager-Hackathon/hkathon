# GET /api/v1/staff/events/{eventId}/lecturers/available

> Staff lấy danh sách Lecturer chưa được phân công vào event.

## Nghiệp vụ
- Staff phải được phân công vào event tương ứng.
- Chỉ trả về user có role = `Lecturer`.
- Chỉ lấy Lecturer chưa được assign vào event này.
- Staff chỉ có thể assign Lecturer với EventRole là `Judge` hoặc `Mentor`.

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
| PageIndex | int | Không (mặc định 1) | 1 | Trang hiện tại |
| PageSize | int | Không (mặc định 10) | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "lecturer@example.com",
        "firstName": "Nguyen",
        "lastName": "Van A",
        "avatarUrl": "https://example.com/avatar.jpg",
        "college": "Dai hoc Bach Khoa",
        "phoneNumber": "0909123456"
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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Chuyển về trang login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Hiển thị thông báo Không có quyền |

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/events/{eventId}/lecturers/available) — [`admin/assign/get/admin.assign.lecturers.available.md`](../../../admin/assign/get/admin.assign.lecturers.available.md)
