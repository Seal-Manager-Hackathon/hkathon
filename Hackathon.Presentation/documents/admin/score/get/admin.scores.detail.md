# GET /api/v1/admin/scores/{scoreId}

> Admin xem chi tiết 1 lượt chấm (score) — kèm track chấm, danh sách điểm từng tiêu chí.

## Phân quyền
- ✅ Admin

## Request

| Param   | Kiểu | Bắt buộc | Ví dụ |
|---------|------|----------|-------|
| scoreId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "scoreId": "guid",
    "submissionId": "guid",
    "assignTrackId": "guid",
    "trackTitle": "Track A",
    "totalScore": 85.5,
    "isRetake": false,
    "retakeFromScoreId": null,
    "isMock": false,
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
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | scoreId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
