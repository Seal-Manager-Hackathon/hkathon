# PATCH /api/v1/judge/scores/{scoreId}

> Judge sửa toàn bộ điểm đã chấm. Trả về paginated danh sách score items kèm flag `isUpdated`, `scoreItemId`, `submissionId`, `gradedByUserId`.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Chỉ judge đã tạo score mới được sửa.
- **Chỉ sửa được khi event chưa kết thúc**: nếu `Event.EndTime` ≤ now → lỗi 400.
- **Chỉ sửa items được gửi lên** — các items không gửi trong request giữ nguyên giá trị cũ.
- **`TotalScore` tự động tính lại = SUM tất cả ScoreItems**.
- **Response trả về paginated score items** với `isUpdated: true` cho items vừa sửa, `false` cho items giữ nguyên.

## Phân quyền
- ✅ Judge — chỉ sửa score của mình

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| scoreId | Guid | ID của score |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

### Body
```json
{
  "scores": [
    {
      "criteriaItemId": "guid",
      "score": 28.0,
      "comment": "Sua lai"
    }
  ]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| scores | array | Có | Danh sách item cần sửa |
| scores[].criteriaItemId | Guid | Có | ID criteria item |
| scores[].score | decimal | Có | Điểm mới |
| scores[].comment | string | No | Nhận xét mới |

## Response (200)
```json
{
  "data": {
    "scoreId": "guid",
    "items": [
      {
        "scoreItemId": "guid",
        "scoreId": "guid",
        "submissionId": "guid",
        "criteriaItemId": "guid",
        "criteriaItemName": "Tinh sang tao",
        "score": 28.0,
        "comment": "Sua lai",
        "gradedByUserId": "guid",
        "isUpdated": true
      },
      {
        "scoreItemId": "guid",
        "scoreId": "guid",
        "submissionId": "guid",
        "criteriaItemId": "guid",
        "criteriaItemName": "Ky thuat",
        "score": 18.0,
        "comment": "Tot",
        "gradedByUserId": "guid",
        "isUpdated": false
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Score Updated Successfully",
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
| `scoreItemId` | ID của score item |
| `scoreId` | ID của lượt chấm (Score) |
| `submissionId` | ID của bài nộp |
| `criteriaItemId` | ID của tiêu chí trong template |
| `criteriaItemName` | Tên tiêu chí |
| `score` | Điểm |
| `comment` | Nhận xét |
| `gradedByUserId` | ID của người chấm (judge) |
| `isUpdated` | `true` = item vừa được sửa trong request này, `false` = giữ nguyên |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Event Has Ended. Cannot Update Score. | Event đã kết thúc |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Authorized to Update This Score | Score của judge khác |
| 404 | Score Not Found | scoreId ko tồn tại |
