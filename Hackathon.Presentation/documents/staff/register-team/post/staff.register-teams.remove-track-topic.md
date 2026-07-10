# POST /api/v1/staff/register-teams/{registerTeamId}/remove-track-topic

> Staff xóa track và topic đã gán cho 1 register team (set về null).

## Nghiệp vụ
- Set TrackId = null, TopicId = null
- Không kiểm tra register team có submission hay không -> cẩn thận khi dùng

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Staff không được phân công vào event | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId ko tồn tại | Báo "Không tìm thấy đơn đăng ký" |

> **Ref:** [Admin API tương ứng](/api/v1/admin/register-team/post/admin.register-teams.remove-track-topic.md)
