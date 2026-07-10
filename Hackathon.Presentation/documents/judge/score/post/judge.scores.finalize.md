# POST /api/v1/judge/scores/{scoreId}/finalize

> Judge finalize điểm (xác nhận không sửa được nữa).

**Controller:** `JudgeController`

## Nghiệp vụ

- Finalize = xác nhận điểm đã chấm, ko cho sửa nữa (cập nhật UpdatedAt).
- Chỉ judge tạo score mới finalize được.

## Phân quyền
- ✅ Judge — chỉ finalize score của mình

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| scoreId | Guid | ID của score |

## Response (200)
```json
{
  "data": "Score Finalized Successfully",
  "message": "Score Finalized Successfully",
  "status": 200,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Authorized to Finalize This Score | Score của judge khác |
| 404 | Score Not Found | scoreId ko tồn tại |
