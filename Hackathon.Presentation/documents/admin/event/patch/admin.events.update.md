# PATCH /api/v1/admin/events/{eventId}

> Admin cập nhật thông tin event. Hỗ trợ partial update — chỉ gửi field nào muốn thay đổi.

**Controller:** [AdminEventController.cs](Controllers/Admin/AdminEventController.cs)

## Nghiệp vụ

- Cập nhật thông tin event: tên, mô tả, thời gian, giới hạn team, trạng thái, ...
- **Hỗ trợ partial update**: field nào null/ko gửi → giữ nguyên giá trị cũ.
- **Event đã Closed**: CHỈ được sửa IsDisable. Các field khác đều bị từ chối.
- **Status transitions hợp lệ**:
  - `Draft` → `Published` ✅ (tự động check setup complete)
  - `Published` → `Closed` ✅
  - `Draft` → `Closed` ❌ (phải qua Published trước)
  - `Published` → `Draft` ❌ (ko hạ cấp)
  - `Closed` → bất kỳ ❌ (đã kết thúc)
- **IsDisable theo từng trạng thái**:
  - `Draft`: mặc định `IsDisable = true` khi tạo. Muốn đổi thành `false` (visible) → phải setup đủ (giống điều kiện publish).
  - `Published`: thoải mái đổi IsDisable.
  - `Closed`: thoải mái đổi IsDisable.
- Format request là `application/json`.

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event cần update |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Name | string | No | Tên event |
| Description | string | No | Mô tả |
| StartTime | datetime | No | Thời gian bắt đầu (phải > hiện tại) |
| EndTime | datetime | No | Thời gian kết thúc (phải > StartTime) |
| RegisterLimitTime | datetime | No | Hạn đăng ký (phải giữa StartTime và EndTime) |
| LimitTeam | int | No | Giới hạn số team |
| MinMember | int | No | Số thành viên tối thiểu mỗi team |
| MaxMember | int | No | Số thành viên tối đa mỗi team |
| IsDisable | bool | No | Ẩn/hiện event. Draft muốn show → phải setup đủ |
| Status | string | No | `Draft`, `Published`, `Closed` |
| Season | string | No | `Spring`, `Summer`, `Autumn`, `Winter` |

## Response (200)
```json
{
  "data": null,
  "message": "Event Updated Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-12T12:00:00Z"
}
```

## Status & IsDisable Logic

```
Tạo event → Draft + IsDisable = true (mặc định)
                │
                ├── IsDisable = false → check setup đủ → mới cho chuyển
                │                       chưa đủ → 400 "Cannot Enable..."
                │
                ├── Draft → Published → IsDisable giữ nguyên
                │        
Published ────── IsDisable thoải mái true/false
    │
    └── Published → Closed → IsDisable thoải mái
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | End Time Must Be After Start Time | EndTime <= StartTime | Báo lỗi thời gian |
| 400 | Start Time Must Be After Current Time | StartTime <= hiện tại | Báo lỗi thời gian |
| 400 | Register Limit Time Must Be After Start Time | RegisterLimitTime <= StartTime | Báo lỗi thời gian |
| 400 | Register Limit Time Must Be Before End Time | RegisterLimitTime >= EndTime | Báo lỗi thời gian |
| 400 | Invalid Season. Must Be: Spring, Summer, Autumn, Winter | Season sai enum | Validation form |
| 400 | Invalid Status. Must Be: Draft, Published, Closed | Status sai enum | Validation form |
| 400 | Cannot Update A Closed Event | Event đã Closed, gửi field khác IsDisable | Disable form |
| 400 | Cannot Change Status From Published Back To Draft | Hạ status từ Published → Draft | Disable nút Draft |
| 400 | Cannot Close A Draft Event Directly. Publish It First | Draft → Closed | Yêu cầu publish trước |
| 400 | Cannot Publish Event. Setup Not Complete. Missing: ... | Draft → Published nhưng thiếu field | Báo field thiếu |
| 400 | Cannot Enable Draft Event. Setup Not Complete. Missing: ... | Draft muốn show (IsDisable=false) nhưng chưa setup đủ | Báo field thiếu |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Event Not Found | eventId ko tồn tại | Báo "Ko tìm thấy event" |
