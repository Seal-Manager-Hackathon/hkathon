# GET /api/v1/staff/submissions/{submissionId}/grader-scores

> Lấy danh sách điểm của các grader cho một submission.

## Nghiệp vụ
- Staff chỉ xem được điểm thuộc event mình được phân công
- Phân trang: PageIndex, PageSize
- Mỗi score là một grader chấm bài này

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| submissionId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của submission |

### Query Parameters
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| pageIndex | int | Không | `1` | Mặc định = 1 |
| pageSize | int | Không | `10` | Mặc định = 10 |

## Response (200)
```json
{
  "data": {
    "submissionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "scores": [
      {
        "scoreId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "submissionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "assignTrackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "trackTitle": "AI",
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicTitle": "Chatbot",
        "totalScore": 85.5,
        "isRetake": false,
        "retakeFromScoreId": null,
        "isMock": false,
        "createdAt": "2026-07-08T12:00:00Z",
        "updatedAt": "2026-07-08T12:00:00Z"
      }
    ],
    "totalCount": 3,
    "pageIndex": 1,
    "pageSize": 10
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
| 404 | Resource Not Found | submissionId không tồn tại | Hiển thị thông báo |
