# GET /api/v1/admin/score-items/{scoreItemId}

> Admin xem chi tiết 1 score item (điểm 1 tiêu chí trong 1 lượt chấm).

## Giải thích nghiệp vụ

**ScoreItem** là đơn vị nhỏ nhất trong hệ thống chấm điểm — điểm số + nhận xét cho 1 tiêu chí.

Khi admin muốn xem cụ thể:
- 1 judge chấm tiêu chí "Sáng tạo" bao nhiêu điểm?
- Có comment gì?
- Judge đó là ai?

Thì dùng API này.

```
Score (lượt chấm)
  └── ScoreItem (1 tiêu chí) ← API này
        └── GradedBy (thông tin người chấm)
```

## Phân quyền
- ✅ Admin

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
      "email": "lecturer@email.com",
      "firstName": "Nguyễn",
      "lastName": "Văn B"
    },
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `scoreItemId` | ID của score item này |
| `scoreId` | ID của lượt chấm (Score) chứa item này |
| `criteriaItemId` | ID của tiêu chí trong criteria template |
| `criteriaName` | Tên tiêu chí (VD: "Tính sáng tạo", "Kỹ thuật", "Thuyết trình") |
| `score` | Điểm judge chấm cho tiêu chí này |
| `comment` | Nhận xét của judge |
| `assignTrackId` | Track judge được assign để chấm |
| `assignEventId` | ID của AssignEvent — record phân công judge vào event |
| `gradedBy` | Thông tin người chấm (judge) |

### gradedBy

| Field | Ý nghĩa |
|-------|---------|
| `userId` | ID người chấm (lecturer — EventRole = Judge) |
| `email` | Email |
| `firstName` / `lastName` | Tên |

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Resource Not Found | scoreItemId không tồn tại |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
