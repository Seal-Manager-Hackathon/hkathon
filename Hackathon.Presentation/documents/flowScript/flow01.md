# Flow 01 — Setup Demo Data (Demo Data API)

> **Mục đích:** Admin gọi 1 API duy nhất để tự động sinh dữ liệu demo: users, teams, đăng ký event, assign track/topic, approve, nộp bài cho round đầu. Dùng để demo sản phẩm trực quan.

## Kiến trúc

- **Controller mới:** `AdminDemoDataController` — `POST /api/v1/admin/demo-data/setup`
- **Service mới:** `DemoDataService` — orchestrator gọi các repository + service hiện có
- **Không seed, không bypass logic** — dùng đúng entity, repository, validation của hệ thống
- **Chỉ Admin** mới gọi được (JWT + RoleEnum.Admin)

## Flow xử lý

```
Admin gọi POST demo-data/setup
  → Tạo 13 users Student (profile đầy đủ)
  → Tạo 5 teams, mỗi team 2-3 thành viên
  → Team leader register event (Pending)
  → Assign track/topic cho từng team
  → Approve từng team (auto vào Round 1)
  → Nộp bài dummy cho Round 1
  → Trả về response tổng hợp
```

## API

### Request

```
POST /api/v1/admin/demo-data/setup
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "eventId": "guid-của-event",
  "trackId": "guid-của-track",
  "topicId": "guid-của-topic (optional)"
}
```

### Response 200

```json
{
  "data": {
    "users": [
      { "id": "guid", "email": "demouser0@demo.com", "firstName": "Demo", "lastName": "User00" }
    ],
    "teams": [
      {
        "id": "guid",
        "name": "Demo Team 01",
        "members": [
          { "userId": "guid", "email": "demouser0@demo.com", "isLeader": true }
        ]
      }
    ],
    "registerTeams": [
      { "id": "guid", "teamName": "Demo Team 01", "status": "Approved" }
    ],
    "submissions": [
      { "id": "guid", "teamName": "Demo Team 01", "roundNo": 1, "url": "https://github.com/demo-submissions/team-01" }
    ]
  }
}
```

## Dữ liệu được tạo

### Users (13 students)

| Index | Email | LastName | StudentId | Phone |
|:-----:|-------|----------|-----------|-------|
| 0 | demouser0@demo.com | User00 | SE0000 | 0900000000 |
| 1 | demouser1@demo.com | User01 | SE0001 | 0900000001 |
| ... | ... | ... | ... | ... |
| 12 | demouser12@demo.com | User12 | SE0012 | 0900000012 |

> Password mặc định: `string`. College: `FPT University`. FirstName: `Demo`. Avatar: `https://robohash.org/{email}`.

### Teams và members

| Team | Tên | Members (Leader đầu) |
|:----:|-----|------|
| 1 | Demo Team 01 | User00 (L), User01, User02 |
| 2 | Demo Team 02 | User03 (L), User04 |
| 3 | Demo Team 03 | User05 (L), User06, User07 |
| 4 | Demo Team 04 | User08 (L), User09 |
| 5 | Demo Team 05 | User10 (L), User11, User12 |

## Chuẩn bị trước khi gọi API

Event cần có đủ cấu hình trước khi gọi:

1. **Event** đã tạo, đã setup đầy đủ (Season, Description, LimitTeam, MinMember, MaxMember)
2. **Track** đã tạo, thuộc event đó
3. **Round 1** đã tạo, thuộc event đó
4. Event có thể ở trạng thái Draft

### Các API cần gọi tay (1 lần)

```bash
# 1. Tạo event
POST /api/v1/admin/events
{
  "name": "Demo Hackathon 2026",
  "description": "Demo event for testing",
  "startTime": "2026-01-01T00:00:00Z",
  "endTime": "2026-12-31T23:59:59Z",
  "limitTeam": 10,
  "minMember": 2,
  "maxMember": 5,
  "season": "Summer"
}

# 2. Tạo round 1
POST /api/v1/admin/events/{eventId}/rounds
{
  "name": "Round 1",
  "startTime": "2026-01-01T00:00:00Z",
  "endTime": "2026-12-31T23:59:59Z",
  "startSubmission": "2026-01-01T00:00:00Z",
  "endSubmission": "2026-12-31T23:59:59Z",
  "limitTeam": 10
}

# 3. Tạo track
POST /api/v1/admin/events/{eventId}/tracks
{
  "title": "Main Track",
  "description": "Main competition track",
  "maxTeam": 10
}

# 4. (Optional) Tạo topic
POST /api/v1/admin/tracks/{trackId}/topics
{
  "title": "AI Application",
  "description": "AI Application topic"
}

# 5. Publish event
PATCH /api/v1/admin/events/{eventId}
{
  "status": "Published",
  "isDisable": false
}
```

## Files implementation

| File | Mục đích |
|------|----------|
| `Hackathon.Application/Services/Admin/DemoData/IDemoDataService.cs` | Interface |
| `Hackathon.Application/Services/Admin/DemoData/Service.cs` | Orchestrator |
| `Hackathon.Application/Services/Admin/DemoData/Request.cs` | Request DTO |
| `Hackathon.Application/Services/Admin/DemoData/Response.cs` | Response DTO |
| `Hackathon.Presentation/Controllers/Admin/AdminDemoDataController.cs` | Controller |
| `Hackathon.Application/Services/Admin/DependencyInjection.cs` | DI registration |
