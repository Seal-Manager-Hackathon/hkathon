# GET /api/v1/student/teams/count

> Đếm số lượng teams. Student chỉ thấy team active (IsDisable = false).

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/count)

## Nghiệp vụ
- Student: **mặc định IsDisable = false**, chỉ đếm team active.
- Không hỗ trợ lọc IsDisable vì student ko cần thấy team bị disable.

## Phân quyền
- ✅ Student

## Request
Ko cần query params.

### Ví dụ
```
GET /api/v1/student/teams/count
```

## Response (200)
```json
{
  "data": { "total": 15 },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
