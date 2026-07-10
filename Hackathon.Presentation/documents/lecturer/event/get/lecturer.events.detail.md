# GET /api/v1/lecturer/events/{eventId}

> Lecturer xem chi tiết một event mà họ được phân công. Response giống hệt Admin GET /api/v1/admin/events/{eventId}.

## Nghiệp vụ

- Lecturer muốn xem thông tin đầy đủ của một event cụ thể.
- Lecturer phải được phân công vào event đó (Judge hoặc Mentor) thì mới xem được.
- Trả về tất cả field của event (id, name, description, startTime, endTime, registerLimitTime, limitTeam, minMember, maxMember, status, isDisable, numberRound, season, createdAt, updatedAt).

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer) — phải được phân công vào event tương ứng

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| eventId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của event |

## Response (200)
```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Hackathon AI 2026",
    "description": "Cuộc thi trí tuệ nhân tạo",
    "startTime": "2026-07-01T00:00:00Z",
    "endTime": "2026-07-10T00:00:00Z",
    "registerLimitTime": "2026-06-25T00:00:00Z",
    "limitTeam": 50,
    "minMember": 2,
    "maxMember": 5,
    "status": "Published",
    "isDisable": false,
    "numberRound": 2,
    "season": "Summer",
    "createdAt": "2026-05-01T00:00:00Z",
    "updatedAt": "2026-06-01T00:00:00Z"
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
| 403 | You do not have permission to perform this action | Không phải Lecturer | Hiển thị thông báo Không có quyền |
| 404 | Event Not Found or You Are Not Assigned to This Event | EventId không tồn tại hoặc lecturer chưa được phân công | Hiển thị thông báo Không tìm thấy |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId})
