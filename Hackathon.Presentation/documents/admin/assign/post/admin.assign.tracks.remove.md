# POST /api/v1/admin/assign/event-assigns/{assignEventId}/tracks/{trackId}/remove

> Admin xóa 1 track khỏi assign event (hủy phân công track cho user).

## Nghiệp vụ
- Xóa mềm (soft delete) — set `IsDisable = true`, ko xóa hẳn record
- User sẽ không còn được gán vào track đó nữa
- Có thể gán lại sau (vì assign check chỉ kiểm tra `IsDisable == false`)
- Hỗ trợ restore sau này nếu cần
- **Chỉ remove được track đang active (IsDisable = false).** Nếu track đã bị remove trước đó, báo lỗi 404 (Assign Track Not Found).

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của assign event |
| trackId | Guid | ID của track cần xóa khỏi assign event |

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Assign Track Not Found | assign event ko có track này |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |

## Logic
1. Kiểm tra assign track tồn tại (IsDisable == false)
2. Set `IsDisable = true`, `UpdatedAt = now`
