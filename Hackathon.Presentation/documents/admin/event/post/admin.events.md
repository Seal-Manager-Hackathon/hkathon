# POST /api/v1/admin/events

> Admin tạo event mới. Mặc định Status = Draft, IsDisable = true, NumberRound = 0.

## Nghiệp vụ
- StartTime phải > hiện tại
- EndTime phải > StartTime
- RegisterLimitTime (nếu có): StartTime < RegisterLimitTime < EndTime
- Không được truyền Status, IsDisable, NumberRound

## Phân quyền
- ✅ Admin

## Request
```json
{
  "name": "Hackathon 2026",
  "description": "...",
  "startTime": "2026-08-01T00:00:00Z",
  "endTime": "2026-08-10T00:00:00Z",
  "registerLimitTime": "2026-07-25T00:00:00Z",
  "limitTeam": 50,
  "minMember": 3,
  "maxMember": 5,
  "season": "Summer"
}
```
| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| name | ✅ | |
| startTime | ✅ | > Now |
| endTime | ✅ | > StartTime |
| registerLimitTime | ❌ | Nếu có: StartTime < x < EndTime |
| limitTeam | ❌ | ≥ 0 |
| minMember | ❌ | ≥ 0 |
| maxMember | ❌ | ≥ 0 |
| season | ❌ | ⚠️ Enum: Spring, Summer, Autumn, Winter |

> ❌ Status, IsDisable, NumberRound không được truyền vào — mặc định lần lượt là Draft, true, 0.

## Response (201)
```json
{
  "data": null,
  "message": "Event Created Successfully",
  "error": null,
  "isSuccess": true,
  "status": 201,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Start Time Must Be After Current Time | startTime ≤ now | Báo "Thời gian bắt đầu phải sau hiện tại" |
| 400 | End Time Must Be After Start Time | endTime ≤ startTime | Báo "Thời gian kết thúc phải sau thời gian bắt đầu" |
| 400 | Register Limit Time Must Be After Start Time | registerLimitTime ≤ startTime | Báo "Hạn đăng ký phải sau thời gian bắt đầu" |
| 400 | Register Limit Time Must Be Before End Time | registerLimitTime ≥ endTime | Báo "Hạn đăng ký phải trước thời gian kết thúc" |
| 400 | Invalid Season. Must be: Spring, Summer, Autumn, Winter | Season sai | Báo "Mùa không hợp lệ" |
| 400 | Invalid Request Data | Validation lỗi (thiếu field, sai format) | Hiển thị từng field |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
