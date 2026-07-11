# GET /api/v1/judge/score-items/{scoreItemId}

> Judge xem chi tiết 1 score item (điểm 1 tiêu chí trong 1 lượt chấm của chính mình), kèm thông tin track/topic. Giống Admin, auth Judge.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Giống Admin `GET /api/v1/admin/score-items/{scoreItemId}`, khác:
  - Phân quyền: Judge, không phải Admin.
  - Judge chỉ xem được score item thuộc lượt chấm do **chính judge đó** thực hiện.
- **ScoreItem** là đơn vị nhỏ nhất trong hệ thống chấm điểm: điểm số + nhận xét cho 1 tiêu chí.
- Khi judge muốn xem cụ thể:
  - Mình chấm tiêu chí "Sáng tạo" bao nhiêu điểm?
  - Có comment gì?
  - Team đó đăng ký track/topic nào?
- Thông tin `trackId`, `topicId`, `topicTitle` lấy từ `RegisterTeams` (qua ScoreEntity → Submission → RoundDetail → RegisterTeam).
- 403 nếu score item không thuộc về judge đang request.

## Phân quyền
- ✅ Judge — phải là người tạo ra score chứa item này

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| scoreItemId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": {
    "scoreItemId": "guid",
    "scoreId": "guid",
    "criteriaItemId": "guid",
    "assignTrackId": "guid",
    "assignEventId": "guid",
    "criteriaName": "Tính sáng tạo",
    "score": 20,
    "comment": "Ý tưởng tốt",
    "gradedBy": {
      "userId": "guid",
      "email": "judge@email.com",
      "firstName": "Nguyễn",
      "lastName": "Văn A"
    },
    "trackId": "guid",
    "topicId": "guid",
    "topicTitle": "AI trong Y tế",
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
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
| `scoreItemId` | ID của score item này |
| `scoreId` | ID của lượt chấm (Score) chứa item này |
| `criteriaItemId` | ID của tiêu chí trong criteria template |
| `criteriaName` | Tên tiêu chí (VD: "Tính sáng tạo") |
| `score` | Điểm judge chấm cho tiêu chí này |
| `comment` | Nhận xét của judge |
| `assignTrackId` | Track judge được assign để chấm |
| `assignEventId` | ID của AssignEvent — record phân công judge vào event |
| `gradedBy` | Thông tin người chấm (chính judge) |
| `trackId` | ID của track mà team đăng ký |
| `topicId` | ID của topic mà team đăng ký |
| `topicTitle` | Tên topic team đăng ký |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Judge của score item đó |
| 404 | Resource Not Found | scoreItemId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/score-items/{scoreItemId})
