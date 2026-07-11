# PATCH /api/v1/judge/score-items/{scoreItemId}

> Judge sửa 1 item điểm riêng lẻ. Chỉ sửa được trước khi event kết thúc. Trả về thông tin score item + scoreId + submissionId + người chấm.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Route chỉ cần `{scoreItemId}`, không cần `{scoreId}` — score item đã biết nó thuộc score nào.
- Chỉ judge tạo score mới được sửa.
- **Chỉ sửa được khi event chưa kết thúc**: nếu `Event.EndTime` ≤ now → lỗi 400.
- **Chỉ sửa field được gửi lên**: `score` và/hoặc `comment` — field không gửi giữ nguyên.
- **Tự động tính lại TotalScore** của score cha (`SUM` tất cả ScoreItems).

## Phân quyền
- ✅ Judge — chỉ sửa score của mình

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| scoreItemId | Guid | ID của score item |

### Body
```json
{
  "score": 28.0,
  "comment": "Sua lai nhan xet"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| score | decimal | No | Điểm mới (không gửi → giữ nguyên) |
| comment | string | No | Nhận xét mới (không gửi → giữ nguyên) |

## Response (200)
```json
{
  "data": {
    "scoreItemId": "guid",
    "scoreId": "guid",
    "submissionId": "guid",
    "criteriaItemId": "guid",
    "criteriaItemName": "Tinh sang tao",
    "score": 28.0,
    "comment": "Sua lai nhan xet",
    "gradedByUserId": "guid",
    "isUpdated": true
  },
  "message": "Score Item Updated Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| `scoreItemId` | ID của score item vừa sửa |
| `scoreId` | ID của lượt chấm (Score) chứa item này |
| `submissionId` | ID của bài nộp |
| `criteriaItemId` | ID của tiêu chí |
| `criteriaItemName` | Tên tiêu chí |
| `score` | Điểm mới |
| `comment` | Nhận xét mới |
| `gradedByUserId` | ID người chấm (judge) |
| `isUpdated` | Luôn `true` vì API này chỉ update 1 item |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Event Has Ended. Cannot Update Score. | Event đã kết thúc |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Authorized to Update This Score Item | Score của judge khác |
| 404 | Score Item Not Found | scoreItemId không tồn tại |
