# POST /api/v1/judge/submissions/{submissionId}/score

> Judge chấm điểm 1 bài nộp.

**Controller:** `JudgeController`

## Nghiệp vụ

- Judge gửi điểm cho 1 submission.
- Dùng `ScoreSubmissionHelper.CreateScore` — item ko nhập → Score = 0.
- **`TotalScore` do FE gửi lên** (có thể override từ SUM scores).
- Judge phải được assign vào track.

## Phân quyền
- ✅ Judge — phải được assign vào track

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| submissionId | Guid | ID của submission |

### Body
```json
{
  "totalScore": 85.0,
  "scores": [
    {
      "criteriaItemId": "guid",
      "score": 25.0,
      "comment": "Y tuong tot"
    }
  ]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| totalScore | decimal | Có | Tổng điểm (FE tính) |
| scores | array | Có | Danh sách điểm tiêu chí |
| scores[].criteriaItemId | Guid | Có | ID criteria item |
| scores[].score | decimal | Có | Điểm cho tiêu chí |
| scores[].comment | string | No | Nhận xét |

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
| 403 | You Are Not Assigned as Judge for This Track | Ko được assign |
| 400 | No Active Criteria Template Found | Round chưa có template |
| 404 | Submission Not Found | submissionId ko tồn tại |
