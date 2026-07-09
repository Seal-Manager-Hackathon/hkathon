# POST /api/v1/staff/register-teams/{registerTeamId}/revert-previous-round

> Staff đưa team về vòng thi trước đó (VD: vòng 3 -> vòng 2).

## Nghiệp vụ
- Cần ít nhất 2 rounds để revert
- Nếu round hiện tại đã có submission -> không cho revert
- Xóa cứng RoundDetail hiện tại

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
  "data": {
    "registerTeamId": "guid",
    "eventId": "guid",
    "teamId": "guid",
    "teamName": "Tên team",
    "trackId": "guid",
    "trackName": "AI",
    "topicId": "guid",
    "topicName": "Chatbot",
    "roundId": "guid",
    "roundName": "Vòng 1",
    "roundNo": 1
  },
  "message": "Reverted Successfully",
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
| 400 | Cannot Revert. Team Has Only One Round | Team chỉ có 1 round, ko thể revert | Báo "Team chỉ có 1 vòng" |
| 400 | Cannot Revert. Current Round Has Submissions | Round hiện tại có submission | Báo "Vòng hiện tại đã có bài nộp" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Staff không được phân công vào event | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId ko tồn tại | Báo "Không tìm thấy đơn đăng ký" |
