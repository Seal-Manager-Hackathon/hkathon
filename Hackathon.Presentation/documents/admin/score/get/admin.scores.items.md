# GET /api/v1/admin/scores/{scoreId}/items

> Admin lấy danh sách score items của 1 lượt chấm, phân trang. Kèm thông tin track/topic từ team.

## Nghiệp vụ

**ScoreItems** là điểm chi tiết của từng tiêu chí trong 1 lượt chấm.

VD: Judge A chấm bài team X, criteria template có 3 tiêu chí:
- Sáng tạo: 40/50
- Kỹ thuật: 30/30
- Thuyết trình: 15/20

Mỗi tiêu chí là 1 ScoreItem. Khi đó score này có:
- `totalScore` = 40 + 30 + 15 = 85
- 3 ScoreItems tương ứng

Thông tin `trackId`, `topicId`, `topicTitle` được lấy từ `RegisterTeams` của team (qua `ScoreEntity → Submission → RoundDetail → RegisterTeam`).

```
Score (lượt chấm)
  └── ScoreEntity → Submission → RoundDetail → RegisterTeam (trackId, topicId)
  └── ScoreItem 1: Sáng tạo = 40 ← API này
  └── ScoreItem 2: Kỹ thuật = 30
  └── ScoreItem 3: Thuyết trình = 15
```

## Phân quyền
- ✅ Admin

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
          "email": "lecturer@email.com",
          "firstName": "Nguyễn",
          "lastName": "Văn B"
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
  "status": 200,
  "traceId": "00-abc123..."
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
| `gradedBy` | Thông tin người chấm |
| **`trackId`** | **ID của track mà team đăng ký** |
| **`topicId`** | **ID của topic mà team đăng ký** |
| **`topicTitle`** | **Tên topic team đăng ký** |

### gradedBy

| Field | Ý nghĩa |
|-------|---------|
| `userId` | ID người chấm (lecturer role Judge) |
| `email` | Email người chấm |
| `firstName` / `lastName` | Tên người chấm |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
