# GET /api/v1/register-teams/{registerTeamId}/current-round

> Lấy thông tin round hiện tại của 1 register team (team đã đăng ký event).
> Ai cũng dùng được miễn đã đăng nhập.

## Phân quyền
- ✅ Authenticated (chỉ cần đăng nhập)

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
    "teamId": "guid",
    "teamName": "Team ABC",
    "trackId": "guid",
    "trackName": "AI",
    "topicId": "guid",
    "topicName": "Computer Vision",
    "currentRoundId": "guid",
    "currentRoundName": "Vòng 1",
    "currentRoundNo": 1
  },
  "message": "Fetched Successfully",
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
| 401 | Invalid Or Expired Token | Chưa đăng nhập |

## Logic
1. Kiểm tra register team tồn tại (kèm Team, Track, Topic, RoundDetails)
2. Lấy round detail có RoundNo lớn nhất → đó là round hiện tại
3. Trả về team info + track/topic + round hiện tại
