# PATCH /api/v1/judge/scores/{scoreId}

> Judge sửa toàn bộ điểm đã chấm.

**Controller:** `JudgeController`

## Nghiệp vụ

- Chỉ judge đã tạo score mới được sửa.
- `TotalScore` do FE gửi lên.

## Phân quyền
- ✅ Judge — chỉ sửa score của mình

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| scoreId | Guid | ID của score |

### Body (giống POST)
```json
{
  "totalScore": 90.0,
  "scores": [
    {
      "criteriaItemId": "guid",
      "score": 28.0,
      "comment": "Sua lai"
    }
  ]
}
```

## Response (200) — JudgeSubmissionScoreResponse
```json
{
  "data": {
    "scoreId": "guid",
    "submissionId": "guid",
    "assignTrackId": "guid",
    "totalScore": 90.0,
    "isRetake": false,
    "isMock": false,
    "scoreItems": [...]
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Authorized to Update This Score | Score của judge khác |
| 404 | Score Not Found | scoreId ko tồn tại |
