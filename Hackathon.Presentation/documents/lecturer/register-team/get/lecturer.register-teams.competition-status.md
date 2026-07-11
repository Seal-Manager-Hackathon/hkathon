# GET /api/v1/lecturer/register-teams/{registerTeamId}/competition-status

> Lecturer kiểm tra trạng thái thi đấu của 1 register team — còn thi đấu hay đã out, kèm track, topic, round hiện tại.

**Controller:** [LecturerRegisterTeamController.cs](Controllers/Lecturer/LecturerRegisterTeamController.cs)

## Nghiệp vụ

- Kiểm tra register team còn đang thi đấu trong event hay không.
- `isStillCompeting` xác định bằng cách:
  - Lấy **round hiện tại** của event dựa trên UTC Now (so với `StartTime`/`EndTime` của các round).
  - Lấy **max round** team đang tham gia (RoundNo cao nhất từ RoundDetails).
  - Nếu max RoundNo của team >= RoundNo hiện tại → `isStillCompeting = true`.
- Trả về thông tin: track, topic, current round (theo thời gian), max round (team đạt được).

## Phân quyền
- ✅ Lecturer

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
    "teamName": "FTeam",
    "eventId": "guid",
    "eventName": "Hackathon 2026",
    "trackId": "guid",
    "trackName": "Web3",
    "topicId": null,
    "topicName": null,
    "currentRoundId": "guid",
    "currentRoundName": "Vòng 2",
    "currentRoundNo": 2,
    "maxRoundId": "guid",
    "maxRoundName": "Vòng 2",
    "maxRoundNo": 2,
    "isStillCompeting": true
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-12T12:00:00Z"
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `registerTeamId` | ID register team |
| `teamId` / `teamName` | Thông tin team |
| `trackId` / `trackName` | Track team đăng ký |
| `topicId` / `topicName` | Topic team chọn |
| `currentRoundId/Name/No` | Round hiện tại của event (theo UTC Now) |
| `currentRoundId` | Null nếu ko có round nào đang diễn ra |
| `maxRoundId/Name/No` | Round cao nhất team đã tham gia |
| `isStillCompeting` | `true` = còn thi đấu, `false` = đã out |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
