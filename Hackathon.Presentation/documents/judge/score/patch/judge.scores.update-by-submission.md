# PATCH /api/v1/judge/submissions/{submissionId}/scores

> Judge sửa lại điểm của 1 bài nộp đã chấm. Không cần biết scoreId — hệ thống tự tìm score của judge hiện tại cho submission đó.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

> **Note:** API cũ `PATCH /api/v1/judge/scores/{scoreId}` đã được thay thế bằng API này. Giờ chỉ cần truyền `submissionId`.

## Nghiệp vụ

- **Không cần scoreId** — hệ thống tự tìm score của judge (qua token) cho submission đó.
- **Chỉ sửa các criteria items được gửi lên** — các items không gửi giữ nguyên điểm cũ.
- **TotalScore tự động tính lại** = SUM Score items.
- **Chỉ sửa được khi event chưa kết thúc**: nếu `Event.EndTime ≤ now` → lỗi 400.
- **Nếu judge chưa chấm bài này** → lỗi 404.

## Phân quyền
- ✅ Judge — phải là người đã chấm bài này

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| submissionId | Guid | ID của submission cần sửa điểm |

### Body
```json
{
  "scores": [
    {
      "criteriaItemId": "guid",
      "score": 28.0,
      "comment": "Sửa lại điểm sau khi review"
    },
    {
      "criteriaItemId": "guid",
      "score": 18.0,
      "comment": "Giữ nguyên"
    }
  ]
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| scores | array | Có | Danh sách điểm tiêu chí cần sửa |
| scores[].criteriaItemId | Guid | Có | ID criteria item |
| scores[].score | decimal | Có | Điểm mới |
| scores[].comment | string | No | Nhận xét mới (ko gửi → giữ cũ) |

## Query Parameters
| Param | Kiểu | Bắt buộc | Mặc định | Ghi chú |
|-------|------|----------|----------|---------|
| pageIndex | int | Không | 1 | Phân trang danh sách items trả về |
| pageSize | int | Không | 10 | Số items mỗi trang |

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
        "comment": "Sửa lại điểm sau khi review",
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
        "comment": "Giữ nguyên",
        "gradedByUserId": "guid",
        "isUpdated": false
      }
    ],
    "totalCount": 2,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Score Updated Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| scoreId | ID của lượt chấm |
| items[].scoreItemId | ID của item điểm |
| items[].isUpdated | `true` = item này vừa được sửa, `false` = giữ nguyên |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Event Has Ended. Cannot Update Score. | Event đã kết thúc |
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Authorized to Update This Score | Ko phải judge đã chấm |
| 404 | Submission Not Found | submissionId ko tồn tại |
| 404 | You Have Not Graded This Submission Yet | Judge chưa chấm bài này |
