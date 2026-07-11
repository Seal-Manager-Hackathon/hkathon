# GET /api/v1/lecturer/teams

> Lecturer lấy danh sách teams phân trang, có lọc theo keyword, canEdit, thời gian tạo.

**Controller:** [LecturerTeamController.cs](Controllers/Lecturer/LecturerTeamController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/teams`

- Giống Admin `GET /api/v1/admin/teams` về request/response, khác auth là Lecturer.
- **Khác Admin:** Luôn filter `IsDisable = false` — chỉ lấy team đang hoạt động.
- Keyword search theo tên team (contains).
- Lọc CanEdit (true/false).
- Lọc FromDate / ToDate theo CreatedAt.
- Sắp xếp gần nhất trên cùng.
- Phân trang: mặc định pageIndex=1, pageSize=10.

## Phân quyền
- ✅ Lecturer

## Request

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tên team |
| CanEdit | bool | No | - | Lọc theo trạng thái có thể chỉnh sửa |
| FromDate | datetime | No | - | Lọc từ ngày tạo |
| ToDate | datetime | No | - | Lọc đến ngày tạo |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng mỗi trang |

## Response (200)
```json
{
  "data": {
    "teams": [
      {
        "id": "guid",
        "name": "FTeam",
        "canEdit": true,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 42,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Teams Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/teams)
