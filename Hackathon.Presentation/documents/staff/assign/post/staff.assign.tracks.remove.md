# POST /api/v1/staff/event-assigns/{assignEventId}/tracks/{trackId}/remove

> Staff xóa 1 track khỏi Lecturer (hủy phân công track cho Lecturer).

## Nghiệp vụ
- Xóa mềm (soft delete) — set `IsDisable = true`, ko xóa hẳn record
- Lecturer sẽ không còn được gán vào track đó nữa
- Có thể gán lại sau (vì assign check chỉ kiểm tra `IsDisable == false`)
- **Chỉ tác động được lên Lecturer** — không thể xóa track của Staff
- Hỗ trợ restore sau này nếu cần

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

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
| 404 | Assign Track Not Found | assign event ko có track này hoặc đã bị xóa |
| 400 | Can Only Modify Lecturer's Tracks | User không phải Lecturer |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff / không được assign vào event |

## Logic
1. Kiểm tra assign event tồn tại và user là Lecturer
2. Kiểm tra assign track tồn tại (IsDisable == false)
3. Set `IsDisable = true`, `UpdatedAt = now`
