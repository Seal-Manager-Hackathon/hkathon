# POST /api/v1/admin/register-teams/{registerTeamId}/assign-next-round

> Admin gán 1 register team vào round tiếp theo (tự động tìm round kế).

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| registerTeamId | Guid | ID của register team |

## Response (201)
```json
{
  "data": {
    "registerTeamId": "guid",
    "roundId": "guid",
    "roundName": "Vòng 2",
    "roundNo": 2
  },
  "message": "Created Successfully",
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
| 400 | This Is The Last Round. Cannot Assign To Next Round | Đã là round cuối, ko có round kế |
| 400 | Team Is Already Assigned To This Round | Team đã được gán vào round này rồi |
| 401 | Invalid Or Expired Token | Chưa đăng nhập |
| 403 | Forbidden | Ko phải admin |

## Logic
1. Authorize Admin
2. Lấy register team kèm RoundDetails → Round
3. Tìm round detail có RoundNo cao nhất → round hiện tại của team
4. Gọi `GetByEventIdAndRoundNoAsync(eventId, currentRoundNo + 1)` → round tiếp theo
5. Nếu ko có round tiếp theo → throw BadRequest "This Is The Last Round"
6. Nếu team đã có round detail trong round đó → throw BadRequest "Team Is Already Assigned"
7. Tạo mới RoundDetails: RoundId = next round, RegisterTeamId = register team
8. SaveChanges → trả về round info
