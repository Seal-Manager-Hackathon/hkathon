# POST /api/v1/judge/submissions/{submissionId}/scores

> Judge chấm điểm 1 bài nộp. Nếu judge đã chấm bài này trước đó, điểm cũ sẽ **bị ghi đè** (update, ko tạo scope mới). Chỉ chấm được khi round đã kết thúc thời gian nộp bài (EndSubmission) và event chưa kết thúc (EndTime). Tự động kiểm tra chấm đủ tất cả tiêu chí.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- **Không nhập thông tin người chấm** — tự động lấy từ token (currentUser).
- **Chỉ chấm được khi round đã qua EndSubmission**: nếu `EndSubmission` > now → lỗi 400.
- **Chỉ chấm được khi event chưa kết thúc**: nếu `Event.EndTime` ≤ now → lỗi 400.
- **Bắt buộc chấm đủ tất cả tiêu chí** trong criteria template active của round. Nếu thiếu → lỗi 400 kèm tên tiêu chí thiếu.
- **`TotalScore` tự động tính = SUM Score items**, FE ko cần gửi.
- Judge phải được assign vào track.
- **Upsert logic**: Nếu judge đã chấm bài này rồi (có Score với cùng assignTrackId và submissionId), hệ thống sẽ **xóa scoreItems cũ và ghi đè điểm mới**. Không tạo thêm Score (scope) thứ hai — tránh cộng dồn điểm sai.
- **Fix 500 khi chấm lại (12/07/2026)**: Trước đây gọi `UpdateAsync` trên entity đã tracking sau khi xóa ScoreItems → EF graph state conflict → 500. Giờ dùng `ReplaceScoreItemsAsync` — xóa cũ + add mới trong 1 phương thức, không gọi `UpdateAsync` vì entity đã được tracking sẵn.

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
  "scores": [
    {
      "criteriaItemId": "guid",
      "score": 25.0,
      "comment": "Y tuong tot"
    },
    {
      "criteriaItemId": "guid",
      "score": 18.0,
      "comment": "Tot"
    }
  ]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
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
    "totalScore": 43.0,
    "isRetake": false,
    "isMock": false,
    "scoreItems": [
      {
        "criteriaItemId": "guid",
        "criteriaItemName": "Tinh sang tao",
        "score": 25.0,
        "comment": "Y tuong tot"
      },
      {
        "criteriaItemId": "guid",
        "criteriaItemName": "Ky thuat",
        "score": 18.0,
        "comment": "Tot"
      }
    ]
  },
  "message": "Score Submitted Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Submission Period Has Not Ended Yet. Cannot Grade Before EndSubmission. | Round chưa qua EndSubmission |
| 400 | Event Has Ended. Cannot Grade. | Event đã kết thúc (EndTime) |
| 400 | Missing Criteria Items: "Tên tiêu chí 1", "Tên tiêu chí 2" | Chưa chấm đủ hết tiêu chí |
| 400 | No Active Criteria Template Found for This Round | Round chưa có template |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Track | Ko được assign |
| 404 | Submission Not Found | submissionId ko tồn tại |
