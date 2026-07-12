# GET /api/v1/student/teams/my-teams

> Student lấy danh sách team mình đang tham gia, có phân trang, tìm kiếm.

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

## Nghiệp vụ
- Trả về danh sách các team mà user đang là member.
- Chỉ lấy team có Status = Active (ko lấy team đã rời).
- Chỉ lấy team có IsDisable = false.
- Hỗ trợ tìm kiếm theo tên team (keyword, contains, ko phân biệt hoa thường).
- Sắp xếp theo CreatedAt mới nhất trước.
- Mỗi item trả về thông tin: tên team, role (isLeader), số lượng member active.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Student

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm kiếm theo tên team |
| pageIndex | int | No | 1 | Trang hiện tại |
| pageSize | int | No | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "teams": [
      {
        "teamId": "guid",
        "teamDetailId": "guid",
        "name": "FTeam",
        "isLeader": true,
        "canEdit": true,
        "status": "Active",
        "memberCount": 3,
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| teamId | ID của team |
| teamDetailId | ID của bản ghi TeamDetail (dùng để join/leave team) |
| name | Tên team |
| isLeader | User có phải leader ko |
| canEdit | Team có cho phép chỉnh sửa ko |
| status | Trạng thái của member trong team (Active/Inactive) |
| memberCount | Số lượng member đang active trong team |

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Page Index/Page Size invalid | pageIndex/pageSize ko hợp lệ | Fix pagination |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
