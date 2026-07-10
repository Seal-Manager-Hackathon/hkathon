# GET /api/v1/staff/users/{userId}/teams

> Lấy danh sách team của 1 user.

## Nghiệp vụ
- Trả về tất cả teams mà user tham gia
- Hỗ trợ tìm kiếm, lọc status, isDisable
- Phân trang

## Phân quyền
- ✅ Staff

## Request
| Param | Kiểu | Bắt buộc | Mô tả |
|-------|------|----------|-------|
| userId | guid | ✅ (route) | ID của user |
| Keyword | string | ❌ | Search tên team |
| Status | string | ❌ | ⚠️ Enum: Active, Inactive |
| IsDisable | bool | ❌ | Lọc disable |
| PageIndex | int | ❌ | Mặc định 1 |
| PageSize | int | ❌ | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "teams": [
      {
        "teamDetailId": "guid",
        "teamId": "guid",
        "teamName": "FTeam",
        "canEdit": true,
        "isDisable": false,
        "isLeader": true,
        "status": "Active",
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Invalid Status | Status sai | Báo lỗi |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff | Ẩn chức năng |
| 404 | User Not Found | userId không tồn tại | Hiển thị thông báo |

> **Ref:** [Admin API tương ứng](/api/v1/admin/user/get/admin.users.teams.md)