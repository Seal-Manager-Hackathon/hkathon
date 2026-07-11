# GET /api/v1/lecturer/scores/{scoreId}/items

> Lecturer xem danh sách score items (điểm từng tiêu chí) của 1 lượt chấm (giống Admin, auth Lecturer).

**Controller:** [LecturerScoreController.cs](Controllers/Lecturer/LecturerScoreController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/scores/{scoreId}/items`

- Giống hệt Admin `GET /api/v1/admin/scores/{scoreId}/items`, khác auth là Lecturer.
- Mỗi ScoreItem = 1 tiêu chí chấm điểm (VD: Tính sáng tạo 20đ), kèm thông tin judge, track, topic.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request
### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| scoreId | guid | ✅ | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

### Query Parameters
| Param | Kiểu | Bắt buộc | Mặc định |
|-------|------|----------|---------|
| PageIndex | int | ❌ | 1 |
| PageSize | int | ❌ | 10 |

## Response (200)
```json
{
  "data": {
    "scoreId": "guid",
    "items": [
      {
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

> **Ref:** [Admin API tương ứng](/api/v1/admin/scores/{scoreId}/items)
