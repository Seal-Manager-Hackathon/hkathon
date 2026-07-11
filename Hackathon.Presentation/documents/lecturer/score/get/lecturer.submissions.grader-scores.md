# GET /api/v1/lecturer/submissions/{submissionId}/grader-scores

> Lecturer xem danh sách lượt chấm (grader scores) của 1 submission (giống Admin, auth Lecturer).

**Controller:** [LecturerScoreController.cs](Controllers/Lecturer/LecturerScoreController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/submissions/{submissionId}/grader-scores`

- Giống hệt Admin `GET /api/v1/admin/submissions/{submissionId}/grader-scores`, khác auth là Lecturer.
- Trả về danh sách các lượt chấm (Score) của 1 submission, có phân trang.
- Mỗi score = 1 judge chấm bài, gồm totalScore + thông tin judge.

## Phân quyền
- ✅ Lecturer

## Request
### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| submissionId | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

### Query Parameters
| Param | Kiểu | Bắt buộc | Mặc định |
|-------|------|----------|---------|
| PageIndex | int | ❌ | 1 |
| PageSize | int | ❌ | 10 |

## Response (200)
```json
{
  "data": {
    "submissionId": "guid",
    "scores": [
      {
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
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/submissions/{submissionId}/grader-scores)
