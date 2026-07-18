# GET /api/v1/student/events/popular

> Lấy danh sách sự kiện phổ biến (popular events), sắp xếp theo số lượng đội đã đăng ký tham gia.

## Nghiệp vụ
- Chỉ lấy các event có `Status = Published`, không bị disable.
- Sắp xếp theo `ApprovedRegisterTeamCount` giảm dần, nếu bằng thì theo tổng số đội đã đăng ký giảm dần.
- Mỗi event trả kèm số lượng đội đã được duyệt (`ApprovedRegisterTeamCount`).

## Phân quyền
- **Public** — không cần token (student không cần đăng nhập)

## Request

| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| PageIndex | int | ✅ | 1 | Trang hiện tại |
| PageSize | int | ✅ | 10 | Số lượng bản ghi mỗi trang |

Không body.

## Response (200)
```json
{
  "isSuccess": true,
  "status": 200,
  "message": "Fetched",
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon 2026",
        "description": "string",
        "startTime": "2026-01-01T00:00:00Z",
        "endTime": "2026-01-03T00:00:00Z",
        "registerLimitTime": "2025-12-30T00:00:00Z",
        "limitTeam": 50,
        "minMember": 2,
        "maxMember": 5,
        "status": "Published",
        "numberRound": 3,
        "season": "Spring",
        "createdAt": "2025-12-01T00:00:00Z",
        "updatedAt": "2025-12-01T00:00:00Z",
        "approvedRegisterTeamCount": 30
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | `Page Index Must Be Greater Than Zero` / `Page Size Must Be Between 1 And 100` | PageIndex <= 0 hoặc PageSize ngoài [1, 100] | Hiển thị lỗi validate |
