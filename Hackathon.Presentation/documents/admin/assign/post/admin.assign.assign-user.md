# POST /api/v1/admin/assign/events/{eventId}/assign/users

> Admin phân công một user (Staff hoặc Lecturer) vào event, kèm EventRole.

## Nghiệp vụ

Admin muốn gán một user vào event để họ có thể tham gia vận hành (Staff) hoặc chấm điểm/mentor (Lecturer).

- **Không thể** phân công user đã bị **disable** (`IsDisable = true`) hoặc đã bị **ban** (`BanReason != null`)
- Không thể phân công Student hoặc Admin vào event
- **Staff** chỉ được assign EventRole = `Staff`
- **Lecturer** chỉ được assign EventRole = `Judge` hoặc `Mentor`
- Mỗi user chỉ được assign vào một event một lần (kiểm tra duplicate)

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Body (JSON)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| UserId | Guid | Yes | ID của user |
| EventRole | string | Yes | `Staff` (cho Staff), `Judge` hoặc `Mentor` (cho Lecturer) |

## Response (201)

```json
{
  "data": null,
  "message": "Created Successfully",
  "status": 201,
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |
| 404 | User Not Found | UserId không tồn tại |
| 404 | Event Role Not Found | EventRole không tồn tại trong DB |
| 400 | Cannot Assign A Disabled User | User đang bị disable |
| 400 | Cannot Assign A Banned User | User đang bị ban (có BanReason) |
| 400 | Cannot Assign Student To Event | User có role Student |
| 400 | Cannot Assign Admin To Event | User có role Admin |
| 400 | Staff Can Only Be Assigned Staff Role | Staff chọn EventRole không phải Staff |
| 400 | Lecturer Cannot Be Assigned Staff Role | Lecturer chọn EventRole = Staff |
| 400 | Invalid EventRole | EventRole không phải Staff/Judge/Mentor |
| 409 | User Is Already Assigned To This Event | User đã được assign vào event này rồi |

## Logic
1. Kiểm tra user tồn tại
2. Kiểm tra user không bị disable và không bị ban
3. Check role: Staff → chỉ Staff role, Lecturer → Judge/Mentor, Student/Admin → reject
4. Parse EventRole từ request
5. Check chưa có assign event nào cho user + event này
6. Lấy EventRole từ DB và tạo AssignEvents
