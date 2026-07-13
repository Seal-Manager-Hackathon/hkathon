# GET /api/v1/student/notifications/count

> Student đếm số lượng notification họ có quyền xem, có thể lọc theo status.

## Nghiệp vụ

API này cho phép student đếm số notification (Personal/Team/System) dựa trên filter status.

- Nếu không truyền status → đếm tất cả.
- Nếu truyền status → chỉ đếm notification có status tương ứng.
- Notification bị disable (IsDisable = true) không được đếm.
- Access control: System → tất cả, Personal → chủ sở hữu, Team → thành viên team.

## Phân quyền
- ✅ Student (đã đăng nhập, token hợp lệ)

## Request

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| status | string | No | Lọc theo trạng thái: `Pending`, `Unread`, `Read` |

## Response (200)
```json
{
  "data": {
    "total": 12
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| total | Tổng số notification thỏa mãn filter |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 400 | Invalid Status | Status sai |

> **Ref:** [Lecturer API tương ứng](/api/v1/lecturer/notifications/count)
