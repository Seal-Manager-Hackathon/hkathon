# POST /api/v1/admin/rounds/{roundId}/end-round

> Admin kết thúc một round ngay lập tức bằng cách set EndTime thành thời điểm hiện tại.

## Nghiệp vụ

- Admin muốn kết thúc sớm một round đang diễn ra (ví dụ: đã đủ bài nộp, cần chuyển sang round tiếp theo).
- API set `EndTime` của round thành `DateTimeOffset.UtcNow`.
- **Chỉ áp dụng cho round đã bắt đầu và chưa kết thúc.**
- **Không cho phép kết thúc round đã bị disable.**
- **Không cho phép kết thúc round chưa có StartTime hoặc StartTime trong tương lai.**

> **Ref:** [AdminRoundController.cs](Controllers/Admin/AdminRoundController.cs)

## Phân quyền
- ✅ AdminPolicy — chỉ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| roundId | Guid | ID của round cần kết thúc |

Không body, không query params.

## Response (200)
```json
{
  "data": null,
  "message": "Round Ended Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | Round Not Found | roundId không tồn tại |
| 400 | Cannot End A Disabled Round | Round đã bị disable/xóa |
| 400 | Round Cannot Be Ended Before It Starts | Round chưa có StartTime hoặc StartTime trong tương lai |
| 400 | Round Has Already Ended | Round đã có EndTime trước thời điểm hiện tại |
