# PATCH /api/v1/judge/scores/{scoreId}/items/{scoreItemId}

> Judge sửa 1 item điểm riêng lẻ.

**Controller:** `JudgeController`

## Nghiệp vụ

- Sửa điểm/comment của 1 tiêu chí cụ thể trong score.
- **Tự động tính lại TotalScore** của score cha (`SUM(Score)`).
- Chỉ judge tạo score mới được sửa.

## Phân quyền
- ✅ Judge — chỉ sửa score của mình

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| scoreId | Guid | ID của score |
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
| score | decimal | No | Điểm mới |
| comment | string | No | Nhận xét mới |

## Response (200)
```json
{
  "data": {
    "criteriaItemId": "guid",
    "criteriaItemName": "Tinh sang tao",
    "score": 28.0,
    "comment": "Sua lai nhan xet"
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Authorized to Update This Score Item | Score của judge khác |
| 404 | Score Item Not Found | scoreItemId ko tồn tại |
