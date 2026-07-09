# POST /api/v1/staff/register-teams/{registerTeamId}/assign-next-round

> Staff gán 1 register team vào round tiếp theo (tự động tìm round kế).

## Nghiệp vụ
- Lấy round hiện tại của team (RoundNo cao nhất)
- Tìm round tiếp theo dựa trên EventId + RoundNo + 1
- Kiểm tra không trùng RoundDetail
- Tạo RoundDetail mới

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
    "roundName": "Vòng 2",
    "roundNo": 2
  },
  "message": "Assigned Successfully",
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
| 400 | This Is The Last Round. Cannot Assign To Next Round | Đã là round cuối, ko có round kế | Báo "Team đã ở vòng cuối" |
| 400 | Team Is Already Assigned To This Round | Team đã được gán vào round này rồi | Báo "Team đã ở round này" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Staff không được phân công vào event | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId ko tồn tại | Báo "Không tìm thấy đơn đăng ký" |
