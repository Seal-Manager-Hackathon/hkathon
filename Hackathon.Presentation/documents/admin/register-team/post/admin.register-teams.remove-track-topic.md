# POST /api/v1/admin/register-teams/{registerTeamId}/remove-track-topic

> Admin gỡ bỏ track và topic của 1 register team (set TrackId = null, TopicId = null).

## Nghiệp vụ
- Xóa cả track và topic khỏi register team
- Không cần body — chỉ cần route parameter

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

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
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
