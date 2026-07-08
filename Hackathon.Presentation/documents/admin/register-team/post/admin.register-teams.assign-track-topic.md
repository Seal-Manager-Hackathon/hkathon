# POST /api/v1/admin/register-teams/{registerTeamId}/assign-track-topic

> Admin gán track và topic cho 1 register team.

## Nghiệp vụ
- Gán TrackId + TopicId cho register team
- Track phải thuộc cùng event với register team
- Topic (nếu có) phải thuộc track được gán
- Nếu ko truyền TopicId → chỉ gán track, bỏ topic

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

### Body (JSON)
```json
{
  "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| trackId | ✅ | Phải tồn tại và cùng event với register team |
| topicId | ❌ | Nếu có, phải thuộc trackId |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
| 404 | Track Not Found | trackId ko tồn tại |
| 404 | Topic Not Found | topicId ko tồn tại |
| 400 | Track Does Not Belong To The Same Event | track thuộc event khác |
| 400 | Topic Does Not Belong To The Specified Track | topic ko thuộc track |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
