# POST /api/v1/staff/register-teams/{registerTeamId}/assign-track-topic

> Staff gán track và topic cho 1 register team.

## Nghiệp vụ
- Track phải thuộc cùng event với register team
- Topic (nếu có) phải thuộc track được gán
- Nếu ko truyền TopicId -> chỉ gán track, bỏ topic

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Track Does Not Belong To The Same Event | Track thuộc event khác | Báo "Track không thuộc event này" |
| 400 | Topic Does Not Belong To The Specified Track | Topic ko thuộc track | Báo "Topic không thuộc track đã chọn" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Staff không được phân công vào event | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId ko tồn tại | Báo "Không tìm thấy đơn đăng ký" |
| 404 | Track Not Found | trackId ko tồn tại | Báo "Không tìm thấy track" |
| 404 | Topic Not Found | topicId ko tồn tại | Báo "Không tìm thấy topic" |
