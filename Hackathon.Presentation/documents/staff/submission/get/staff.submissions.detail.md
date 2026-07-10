# GET /api/v1/staff/submissions/{submissionId}

> Xem chi tiết một submission.

## Nghiệp vụ
- Staff chỉ xem được submission thuộc event mình được phân công
- Trả về thông tin: team, round, track, topic, URL, mô tả, trạng thái, điểm
- `totalScore` = tổng `TotalScore` các score hợp lệ
- `submittedBy` = leader của team (`IsLeader = true`)
- `judgeCount` = số lượng judge đã chấm

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| submissionId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của submission |

## Response (200)

```json
{
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "roundDetailId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "roundName": "Vòng 1",
    "registerTeamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "teamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "teamName": "Team A",
    "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "trackTitle": "AI",
    "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "topicTitle": "Chatbot",
    "url": "https://example.com/submission.pdf",
    "description": "Bài nộp vòng 1",
    "status": "Submitted",
    "submittedAt": "2026-07-08T10:00:00Z",
    "isRegrade": false,
    "submittedBy": {
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "leader@example.com",
      "firstName": "Nguyễn",
      "lastName": "Văn A"
    },
    "totalScore": 85.5,
    "judgeCount": 3,
    "createdAt": "2026-07-08T10:00:00Z",
    "updatedAt": "2026-07-08T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-09T12:00:00Z"
}
```

## Lỗi

| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Resource Not Found | submissionId không tồn tại | Hiển thị thông báo không tìm thấy |

> **Ref:** [Admin API tương ứng](/api/v1/admin/submission/get/admin.submissions.detail.md)
