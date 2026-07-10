# GET /api/v1/judge/submissions/{submissionId}/criteria

> Judge lấy tiêu chí chấm điểm cho 1 bài nộp.

**Controller:** `JudgeController`

## Nghiệp vụ

- Dùng active criteria template của round.
- Judge phải được assign vào track của submission.

## Phân quyền
- ✅ Judge — phải được assign vào track

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| submissionId | Guid | ID của submission |

## Response (200)
```json
{
  "data": {
    "submissionId": "guid",
    "roundId": "guid",
    "templateId": "guid",
    "templateTitle": "Criteria Chung",
    "criteriaItems": [
      {
        "id": "guid",
        "name": "Tinh sang tao",
        "description": "...",
        "maxScore": 30.0
      }
    ]
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Track | Judge ko được assign |
| 404 | Submission Not Found | submissionId ko tồn tại |
