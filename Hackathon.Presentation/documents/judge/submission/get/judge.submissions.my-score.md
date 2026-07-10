# GET /api/v1/judge/submissions/{submissionId}/my-score

> Judge xem lại điểm mình đã chấm cho 1 bài nộp.

**Controller:** `JudgeController`

## Nghiệp vụ

- Trả về score của judge hiện tại cho submission này.
- Nếu chưa chấm → trả về null (200 với data = null).
- ScoreItems chứa danh sách điểm từng tiêu chí.

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
    "scoreId": "guid",
    "submissionId": "guid",
    "assignTrackId": "guid",
    "retakeFromScoreId": null,
    "totalScore": 85.0,
    "isRetake": false,
    "isMock": false,
    "scoreItems": [
      {
        "criteriaItemId": "guid",
        "criteriaItemName": "Tinh sang tao",
        "score": 25.0,
        "comment": "Y tuong tot"
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
