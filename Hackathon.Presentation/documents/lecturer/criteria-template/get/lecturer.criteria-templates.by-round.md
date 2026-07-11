# GET /api/v1/lecturer/rounds/{roundId}/criteria-templates

> Lecturer lấy danh sách criteria template của round (chỉ active và không bị disable).

**Controller:** [LecturerCriteriaTemplateController.cs](Controllers/Lecturer/LecturerCriteriaTemplateController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/rounds/{roundId}/criteria-templates`

- Giống hệt Admin `GET /api/v1/admin/rounds/{roundId}/criteria-templates`, khác auth là Lecturer.
- **Khác Admin:** Luôn filter `IsActive = true && IsDisable = false` — chỉ lấy template đang được dùng.
- Hỗ trợ tìm kiếm theo tiêu đề (contains, không phân biệt hoa thường).
- Sắp xếp theo CreatedAt giảm dần (mới nhất lên đầu).
- Phân trang: mặc định pageIndex=1, pageSize=10.
- 404 nếu roundId không tồn tại.

## Phân quyền
- ✅ Lecturer

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| roundId | Guid | ID của round |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| Keyword | string | No | - | Tìm kiếm theo tiêu đề template |
| PageIndex | int | No | 1 | Trang hiện tại |
| PageSize | int | No | 10 | Số lượng item mỗi trang |

## Response (200)
```json
{
  "data": {
    "templates": [
      {
        "id": "guid",
        "roundId": "guid",
        "title": "Tiêu chí chấm điểm vòng 1",
        "description": "Đánh giá ý tưởng",
        "isDisable": false,
        "isActive": true,
        "createdAt": "2026-07-07T12:00:00Z",
        "updatedAt": "2026-07-07T12:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "traceId": "..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |
| 404 | Resource Not Found | roundId không tồn tại |
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ |

> **Ref:** [Admin API tương ứng](/api/v1/admin/rounds/{roundId}/criteria-templates)
