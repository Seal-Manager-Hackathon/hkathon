# GET /api/v1/student/notifications/unread-count

> Student đếm số notification chưa đọc (status = Unread).

## Nghiệp vụ

- Chỉ đếm notification có `Status == Unread`.
- Chỉ đếm notification student có quyền xem (System, Personal, Team).
- Notification bị disable không được đếm.

## Phân quyền
- ✅ Student (đã đăng nhập, token hợp lệ)

## Request
Không query params.

## Response (200)
```json
{
  "data": {
    "count": 5
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| count | Số notification chưa đọc |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |

> **Ref:** [Lecturer API tương ứng](/api/v1/lecturer/notifications/unread-count)
