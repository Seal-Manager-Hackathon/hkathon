# POST /api/v1/admin/events/{eventId}/rounds

> Admin tạo round mới cho event. RoundNo tự động tăng dần.

## Nghiệp vụ

**Tự động tính RoundNo:**
- Lấy roundNo lớn nhất hiện tại của event → +1
- Nếu chưa có round nào → mặc định RoundNo = 1
- **Không cho nhập RoundNo**

**Cập nhật NumberRound của event:**
- Khi tạo round thành công → `NumberRound` của event tự động +1

**Ràng buộc thời gian:**
| Điều kiện | Mô tả |
|-----------|-------|
| StartTime < EndTime | Thời gian bắt đầu phải trước thời gian kết thúc |
| StartTime ≥ RegisterLimitTime của event | Nếu event có RegisterLimitTime |
| StartTime ≥ StartTime của event, EndTime ≤ EndTime của event | Round phải nằm trong khung thời gian event |
| StartSubmission ≥ StartTime (nếu có) | Thời gian bắt đầu nộp bài phải ≥ thời gian bắt đầu round |
| EndSubmission ≤ EndTime (nếu có) | Thời gian kết thúc nộp bài phải ≤ thời gian kết thúc round |
| LimitTeam ≥ 1 (nếu có) | Giới hạn team tối thiểu là 1 |

**Check round trước đó:**
- Nếu đã có round trước (RoundNo - 1): StartTime của round hiện tại **phải ≥ EndTime của round trước**
- Đảm bảo không bị chồng chéo thời gian giữa các round

## Phân quyền
- ✅ Admin

## Request
```json
{
  "name": "Vòng 1",
  "description": "...",
  "startTime": "2026-08-01T00:00:00Z",
  "endTime": "2026-08-03T00:00:00Z",
  "startSubmission": "2026-08-01T00:00:00Z",
  "endSubmission": "2026-08-03T00:00:00Z",
  "limitTeam": 20
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| name | ✅ | |
| description | ❌ | |
| startTime | ✅ | < EndTime, ≥ RegisterLimitTime event, ≥ StartTime event, ≥ EndTime round trước |
| endTime | ✅ | > StartTime, ≤ EndTime event |
| startSubmission | ❌ | ≥ StartTime |
| endSubmission | ❌ | ≤ EndTime |
| limitTeam | ❌ | ≥ 1 |

> ❌ `roundNo` không được truyền vào — tự động tính.

## Response (201)
```json
{
  "data": null,
  "message": "Round Created Successfully",
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
| 400 | Round End Time Must Be After Start Time | endTime ≤ startTime | Báo "Thời gian kết thúc phải sau thời gian bắt đầu" |
| 400 | Start Submission Must Be After Or Equal Start Time | startSubmission < startTime | Báo "Thời gian nộp bài phải sau thời gian bắt đầu" |
| 400 | End Submission Must Be Before Or Equal End Time | endSubmission > endTime | Báo "Thời gian nộp bài phải trước thời gian kết thúc" |
| 400 | Limit Team Must Be At Least 1 | limitTeam < 1 | Báo "Giới hạn team tối thiểu là 1" |
| 400 | Round Time Must Be Within Event Time Range | startTime < event.startTime hoặc endTime > event.endTime | Báo "Thời gian round phải nằm trong thời gian event" |
| 400 | Round Start Time Must Be After Event Register Limit Time | startTime < event.registerLimitTime | Báo "Thời gian bắt đầu round phải sau hạn đăng ký" |
| 400 | Round Start Time Must Be After Or Equal Previous Round End Time | startTime < endTime của round trước | Báo "Round mới phải bắt đầu sau khi round trước kết thúc" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId không tồn tại | Báo "Không tìm thấy sự kiện" |
