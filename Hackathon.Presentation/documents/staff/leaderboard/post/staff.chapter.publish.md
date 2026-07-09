# POST /api/v1/staff/events/chapter/{year}/leaderboard/publish

> Staff công bố leader board chapter cho một năm — set IsDisable=false, IsPublished=true.

## Nghiệp vụ

Staff muốn công bố kết quả leader board của một năm. Hệ thống sẽ:

1. Lấy tất cả leader board có `Year == year` mà Staff được assign vào event tương ứng
2. Set `IsDisable = false`, `IsPublished = true` cho các leader board đó
3. Sau khi publish, leader board sẽ xuất hiện ở các API GET leader board

## Phân quyền
- ✅ Staff (phải được phân công vào ít nhất 1 event trong năm)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| year | int | Năm cần publish leader board |

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
| 403 | You Are Not Assigned to Any Event In This Year | Staff không được assign event nào trong năm |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff |
