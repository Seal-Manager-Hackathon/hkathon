# GET /api/v1/lecturer/rounds/{roundId}

> Lecturer xem chi tiết 1 round.
> **Controller:** `LecturerRoundController` — `GET /api/v1/lecturer/rounds/{roundId}`

## Nghiệp vụ

- Lecturer xem thông tin chi tiết của 1 round.
- Response giống hệt Admin `GET /api/v1/admin/rounds/{roundId}`.
- Dùng lại Base Round API (tất cả role đã đăng nhập đều dùng chung).

## Phân quyền
- ✅ Lecturer

## Request
| Parameter | Type | Bắt buộc | Ghi chú |
|-----------|------|----------|---------|
| roundId | Guid | ✅ | ID của round |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "name": "Vòng 1",
    "description": "Vòng loại",
    "roundNo": 1,
    "startTime": "2026-07-10T00:00:00Z",
    "endTime": "2026-07-15T00:00:00Z",
    "startSubmission": "2026-07-11T00:00:00Z",
    "endSubmission": "2026-07-14T00:00:00Z",
    "limitTeam": 50,
    "isDisable": false,
    "createdAt": "2026-07-01T00:00:00Z",
    "updatedAt": "2026-07-01T00:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | roundId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId})
