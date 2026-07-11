# GET /api/v1/lecturer/teams/count

> Lecturer đếm số lượng teams active. Luôn đếm team có IsDisable = false.

**Controller:** [LecturerTeamController.cs](Controllers/Lecturer/LecturerTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/teams/count`

- Giống Admin `GET /api/v1/admin/teams/count` về response format, khác auth là Lecturer.
- **Khác Admin:** Luôn đếm team có `IsDisable = false` — bỏ qua filter IsDisable từ request.

## Phân quyền
- ✅ Lecturer

## Request

Không có tham số.

## Response (200)
```json
{
  "data": { "total": 15 },
  "message": "Team Count Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams/count)
