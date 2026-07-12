# GET /api/v1/student/rounds/{roundId}

> Student xem chi tiết 1 round.

**Controller:** [StudentRoundController.cs](Controllers/Student/StudentRoundController.cs)

## Nghiệp vụ
- Xem chi tiết round: tên, thời gian, roundNo, limitTeam.
- Nếu round bị disable (`IsDisable = true`) → 404.
- Giống Admin `GET /api/v1/admin/rounds/{roundId}` — response data như nhau.

## Phân quyền
- ✅ Student

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Student | Ẩn chức năng |
| 404 | Round Not Found | roundId ko tồn tại / bị disable | Báo "Không tìm thấy vòng" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId})
