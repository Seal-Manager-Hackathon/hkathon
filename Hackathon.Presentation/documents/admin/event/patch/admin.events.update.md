# PATCH /api/v1/admin/events/{eventId}

> Admin chỉnh sửa thông tin event. Chỉ gửi field cần sửa, null giữ nguyên.

## Nghiệp vụ & điều kiện

**Nguyên tắc chung:**
- Field null → giữ nguyên giá trị cũ
- **Không được chỉnh:** `NumberRound`
- Validate thời gian: StartTime > now, EndTime > StartTime, RegisterLimitTime giữa StartTime và EndTime (chỉ check khi field được gửi lên)

**Khi chuyển IsDisable từ `true` → `false` (bật event):**
1. Check đủ thông tin bắt buộc: Name, StartTime (> now), EndTime (> StartTime), Season
2. Nếu thiếu → báo lỗi `Cannot Enable Event. Missing Required Fields: ...`
3. Nếu đang Status = `Draft` → tự động đổi thành `Published`
4. Sau đó set `IsDisable = false`

**Khi chuyển Status từ `Draft` → `Published`:**
1. Check đủ thông tin bắt buộc giống như trên
2. Nếu thiếu → báo lỗi `Cannot Publish Event. Missing Required Fields: ...`

**Khi chỉnh disable mà không publish:**
- Nếu chỉ `IsDisable = false` + đủ thông tin + status đang Draft → **tự lên Published**
- Nếu `IsDisable = false` + đủ thông tin + status đã Published → chỉ enable, không đổi status

## Phân quyền
- ✅ Admin

## Request
```json
{
  "name": "Hackathon 2026 Updated",
  "description": "Mô tả mới",
  "startTime": "2026-08-01T00:00:00Z",
  "endTime": "2026-08-10T00:00:00Z",
  "registerLimitTime": "2026-07-25T00:00:00Z",
  "limitTeam": 50,
  "minMember": 3,
  "maxMember": 5,
  "isDisable": false,
  "status": "Published",
  "season": "Summer"
}
```

| Field | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| name | string | ❌ | |
| description | string | ❌ | |
| startTime | datetime | ❌ | Phải > now nếu gửi lên |
| endTime | datetime | ❌ | Phải > StartTime nếu gửi lên |
| registerLimitTime | datetime | ❌ | Phải giữa StartTime và EndTime nếu gửi lên |
| limitTeam | int | ❌ | ≥ 0 |
| minMember | int | ❌ | ≥ 0 |
| maxMember | int | ❌ | ≥ 0 |
| isDisable | bool | ❌ | true = ẩn, false = bật (tự publish nếu đủ điều kiện) |
| status | string | ❌ | ⚠️ Enum: Draft, Published, Closed |
| season | string | ❌ | ⚠️ Enum: Spring, Summer, Autumn, Winter |

> ❌ `NumberRound` không được truyền vào.

## Response (200)
```json
{
  "data": null,
  "message": "Event Updated Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Start Time Must Be After Current Time | startTime mới ≤ now | Báo "Thời gian bắt đầu phải sau hiện tại" |
| 400 | End Time Must Be After Start Time | endTime mới ≤ startTime | Báo "Thời gian kết thúc phải sau thời gian bắt đầu" |
| 400 | Register Limit Time Must Be After Start Time | registerLimitTime mới sai | Báo "Hạn đăng ký phải sau thời gian bắt đầu" |
| 400 | Cannot Enable Event. Missing Required Fields: ... | Muốn bật event nhưng thiếu Name/StartTime/EndTime/Season | Bổ sung các field còn thiếu |
| 400 | Cannot Publish Event. Missing Required Fields: ... | Muốn publish nhưng thiếu thông tin | Bổ sung các field còn thiếu |
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai | Báo "Trạng thái không hợp lệ" |
| 400 | Invalid Season. Must be: Spring, Summer, Autumn, Winter | Season sai | Báo "Mùa không hợp lệ" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |

## Luồng trạng thái

```
  Tạo event → Status=Draft, IsDisable=true
       │
       ├── Gửi PATCH với isDisable=false
       │   └── Đủ thông tin? → Status=Published, IsDisable=false
       │   └── Thiếu thông tin → Báo lỗi, yêu cầu bổ sung
       │
       ├── Gửi PATCH với status=Published
       │   └── Đủ thông tin? → Status=Published (IsDisable giữ nguyên)
       │   └── Thiếu thông tin → Báo lỗi, yêu cầu bổ sung
       │
       └── Gửi PATCH với status=Closed
           └── Không check gì thêm
```
