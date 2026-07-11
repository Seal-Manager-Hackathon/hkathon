# GET /api/v1/lecturer/scores/{scoreId}

> Lecturer xem chi tiết 1 lượt chấm (score) của 1 người chấm (giống Admin, auth Lecturer).

**Controller:** [LecturerScoreController.cs](Controllers/Lecturer/LecturerScoreController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/scores/{scoreId}`

- Giống hệt Admin `GET /api/v1/admin/scores/{scoreId}`, khác auth là Lecturer.
- 1 score = 1 lượt chấm của 1 judge cho 1 bài nộp (submission).
- 404 nếu scoreId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| scoreId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "scoreId": "guid",
    "submissionId": "guid",
    "assignTrackId": "guid",
    "trackTitle": "Track A",
    "trackId": "guid",
    "topicId": "guid",
    "topicTitle": "AI trong Y tế",
    "totalScore": 85.5,
    "isRetake": false,
    "retakeFromScoreId": null,
    "isMock": false,
    "gradedBy": {
      "userId": "guid",
      "email": "judge@example.com",
      "firstName": "Nguyen",
      "lastName": "Van A"
    },
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
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | scoreId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/scores/{scoreId})
