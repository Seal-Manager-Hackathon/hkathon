# PATCH /api/v1/admin/rounds/{roundId}

> Admin sửa thông tin round. Không sửa được RoundNo.

## Nghiệp vụ

**Các field không được sửa:**
- ❌ `roundNo` — tự động tạo, không cho nhập
- ❌ `eventId` — round thuộc event nào là cố định

**Ràng buộc thời gian (kiểm tra khi field được gửi lên):**
| Điều kiện | Mô tả |
|-----------|-------|
| StartTime < EndTime | Nếu gửi cả 2 hoặc 1 trong 2 |
| StartTime ≥ RegisterLimitTime của event | Nếu event có RegisterLimitTime |
| StartTime ≥ StartTime của event, EndTime ≤ EndTime của event | Round phải nằm trong khung thời gian event |
| StartSubmission ≥ StartTime | Nếu cả 2 đều có giá trị |
| EndSubmission ≤ EndTime | Nếu cả 2 đều có giá trị |
| LimitTeam ≥ 1 | Nếu gửi lên |

**Check round trước (RoundNo - 1):**
- StartTime của round hiện tại **phải ≥ EndTime** của round trước (nếu có)

**Check round sau (RoundNo + 1):**
- EndTime của round hiện tại **phải ≤ StartTime** của round sau (nếu có)

## Phân quyền
- ✅ Admin

## Request
```json
{
  "name": "Vòng 1 - Cập nhật",
  "description": "...",
  "startTime": "2026-08-01T00:00:00Z",
  "endTime": "2026-08-03T00:00:00Z",
  "startSubmission": "2026-08-01T00:00:00Z",
  "endSubmission": "2026-08-03T00:00:00Z",
  "limitTeam": 25
}
```

| Field | Kiểu | Bắt buộc | Ghi chú |
|-------|------|----------|---------|
| name | string | ❌ | null giữ nguyên |
| description | string | ❌ | null giữ nguyên |
| startTime | datetime | ❌ | Phải tuân thủ các ràng buộc thời gian |
| endTime | datetime | ❌ | Phải tuân thủ các ràng buộc thời gian |
| startSubmission | datetime | ❌ | null giữ nguyên |
| endSubmission | datetime | ❌ | null giữ nguyên |
| limitTeam | int | ❌ | null giữ nguyên, nếu gửi phải ≥ 1 |

> ❌ `roundNo`, `eventId` không được truyền vào.

## Response (200)
```json
{
  "data": null,
  "message": "Round Updated Successfully",
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
| 400 | Round End Time Must Be After Start Time | endTime ≤ startTime | Báo "Thời gian kết thúc phải sau thời gian bắt đầu" |
| 400 | Start Submission Must Be After Or Equal Start Time | startSubmission < startTime sau khi sửa | Báo "Thời gian nộp bài phải sau thời gian bắt đầu" |
| 400 | End Submission Must Be Before Or Equal End Time | endSubmission > endTime sau khi sửa | Báo "Thời gian nộp bài phải trước thời gian kết thúc" |
| 400 | Limit Team Must Be At Least 1 | limitTeam < 1 | Báo "Giới hạn team tối thiểu là 1" |
| 400 | Round Time Must Be Within Event Time Range | startTime < event.startTime hoặc endTime > event.endTime | Báo "Thời gian round phải nằm trong thời gian event" |
| 400 | Round Start Time Must Be After Event Register Limit Time | startTime < event.registerLimitTime | Báo "Thời gian bắt đầu round phải sau hạn đăng ký" |
| 400 | Round Start Time Must Be After Or Equal Previous Round End Time | startTime < endTime của round trước | Báo "Round hiện tại phải bắt đầu sau round trước kết thúc" |
| 400 | Round End Time Must Be Before Or Equal Next Round Start Time | endTime > startTime của round sau | Báo "Round hiện tại phải kết thúc trước round sau bắt đầu" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Round Not Found | roundId không tồn tại | Báo "Không tìm thấy round" |
| 404 | Event Not Found | Event của round không tồn tại (lỗi hệ thống) | Báo "Lỗi dữ liệu" |
