# PATCH api/v1/admin/events/{eventId}

Cập nhật thông tin event.

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Body (JSON)

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Name | string | No | Tên event |
| Description | string | No | Mô tả |
| StartTime | datetime | No | Thời gian bắt đầu |
| EndTime | datetime | No | Thời gian kết thúc |
| RegisterLimitTime | datetime | No | Hạn đăng ký |
| LimitTeam | int | No | Giới hạn số team |
| MinMember | int | No | Số thành viên tối thiểu mỗi team |
| MaxMember | int | No | Số thành viên tối đa mỗi team |
| IsDisable | bool | No | Ẩn/hiện event (hoàn toàn độc lập) |
| Status | string | No | `Draft`, `Published`, `Closed` |
| Season | string | No | `Spring`, `Summer`, `Autumn`, `Winter` |

## Response

```json
{
  "data": null,
  "message": "Event Updated Successfully",
  "traceId": "..."
}
```

## Logic

### IsDisable
- `IsDisable` hoàn toàn **độc lập**, không kéo theo bất kỳ thay đổi nào về status hay setup check
- Chỉ đơn thuần set field `IsDisable` lên entity

### Status: Draft → Published
- Tự động gọi `GET events/{eventId}/setup-check` (method `IsEventSetupComplete`)
- Nếu chưa setup đủ → throw `BadRequestException` kèm danh sách field thiếu
- Nếu đủ → cho phép chuyển thành Published

### Validate thời gian
- `StartTime` phải > hiện tại
- `EndTime` phải > StartTime
- `RegisterLimitTime` phải nằm giữa StartTime và EndTime

## Error

- `401` — Unauthorized
- `404` — Event Not Found
- `400` — Validation lỗi (thời gian, thiếu field khi publish, ...)
