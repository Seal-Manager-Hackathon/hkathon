# GET /api/v1/judge/scores/{scoreId}/items

> Judge xem danh sách score items (điểm từng tiêu chí) của 1 lượt chấm của chính mình, phân trang. Giống Admin, auth Judge.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Giống Admin `GET /api/v1/admin/scores/{scoreId}/items`, khác:
  - Phân quyền: Judge, không phải Admin.
  - Judge chỉ xem được score items của lượt chấm do **chính judge đó** thực hiện.
- **ScoreItems** là điểm chi tiết của từng tiêu chí trong 1 lượt chấm.
- Ví dụ: Judge A chấm bài team X, criteria template có 3 tiêu chí:
  - Sáng tạo: 40/50
  - Kỹ thuật: 30/30
  - Thuyết trình: 15/20
  → Mỗi tiêu chí là 1 ScoreItem, tổng `totalScore` = 85.
- Thông tin `trackId`, `topicId`, `topicTitle` lấy từ `RegisterTeams` (qua ScoreEntity → Submission → RoundDetail → RegisterTeam).
- Mất quyền: Judge ko sở hữu scoreId đó → 403.

## Phân quyền
- ✅ Judge — phải là người tạo ra score đó

## Request

### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| scoreId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

### Query Parameters
| Param | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| pageIndex | int | ❌ | Mặc định 1 |
| pageSize | int | ❌ | Mặc định 10, tối đa 100 |

## Response (200)
```json
{
  "data": {
    "scoreId": "guid",
    "items": [
      {
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
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
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
| `scoreItemId` | ID của item này |
| `scoreId` | ID của lượt chấm chứa item |
| `criteriaItemId` | ID của tiêu chí trong criteria template |
| `criteriaName` | Tên tiêu chí (VD: "Tính sáng tạo", "Kỹ thuật") |
| `score` | Điểm cho tiêu chí này |
| `comment` | Nhận xét của judge cho tiêu chí này |
| `assignTrackId` | Track người chấm được phân công |
| `assignEventId` | Event assign record của người chấm |
| `gradedBy` | Thông tin người chấm (chính judge) |
| `trackId` | ID của track mà team đăng ký |
| `topicId` | ID của topic mà team đăng ký |
| `topicTitle` | Tên topic team đăng ký |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Judge của score đó |
| 404 | Resource Not Found | scoreId không tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/scores/{scoreId}/items)
