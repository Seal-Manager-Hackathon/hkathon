# GET /api/v1/lecturer/score-items/{scoreItemId}

> Lecturer xem chi tiết 1 score item (giống Admin, auth Lecturer).

**Controller:** [LecturerScoreController.cs](Controllers/Lecturer/LecturerScoreController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/score-items/{scoreItemId}`

- Giống hệt Admin `GET /api/v1/admin/score-items/{scoreItemId}`, khác auth là Lecturer.
- ScoreItem là đơn vị nhỏ nhất trong chấm điểm: điểm số + nhận xét cho 1 tiêu chí.
- 404 nếu scoreItemId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| scoreItemId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "scoreItemId": "guid",
    "scoreId": "guid",
    "criteriaItemId": "guid",
    "assignTrackId": "guid",
    "assignEventId": "guid",
    "criteriaName": "Tính sáng tạo",
    "score": 20,
    "comment": "Ý tưởng tốt",
    "gradedBy": {
      "userId": "guid",
      "email": "lecturer@email.com",
      "firstName": "Nguyễn",
      "lastName": "Văn B"
    },
    "trackId": "guid",
    "topicId": "guid",
    "topicTitle": "AI trong Y tế",
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
| 404 | Resource Not Found | scoreItemId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/score-items/{scoreItemId})
