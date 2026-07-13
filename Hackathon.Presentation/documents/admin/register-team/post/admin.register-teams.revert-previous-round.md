# POST /api/v1/admin/register-teams/{registerTeamId}/revert-previous-round

> Admin lùi register team về round trước đó (xóa cứng round detail hiện tại).

## Nghiệp vụ
- Chỉ được revert trước khi event bắt đầu. Nếu event đã bắt đầu, không thể lùi về round trước.

## Phân quyền
- ✅ Admin

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
    "teamName": "Team ABC",
    "trackId": "guid",
    "trackName": "AI",
    "topicId": "guid",
    "topicName": "Computer Vision",
    "roundId": "guid",
    "roundName": "Vòng 1",
    "roundNo": 1
  },
  "message": "Updated Successfully",
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Register Team Not Found | registerTeamId ko tồn tại |
| 400 | Cannot Revert: Current Round Has Submission(s). Please Delete Submissions First | Round hiện tại đã có submission, ko thể quay lại |
| 400 | Team Is Only In One Round. Cannot Revert To Previous Round | Team chỉ có 1 round, ko thể lùi |
| 400 | Cannot Revert To Previous Round After Event Has Started | Event đã bắt đầu | Báo "Sự kiện đã bắt đầu" |
| 401 | Invalid Or Expired Token | Chưa đăng nhập |
| 403 | Forbidden | Ko phải admin |

## Logic
1. Authorize Admin
2. Lấy register team kèm RoundDetails (chỉ lấy các active, ko bị IsDisable)
3. Sắp xếp RoundDetails giảm dần theo RoundNo
4. Nếu chỉ có 1 round → throw BadRequest (ko thể lùi)
5. Check round hiện tại đã có submissions chưa → nếu có → throw BadRequest
6. **Xóa cứng** (`Remove`) round detail hiện tại (ko có submission nên safe)
7. Round trước đó = phần tử thứ 2 trong danh sách (đã sắp xếp)
7. SaveChanges → trả về thông tin round đã lùi về
