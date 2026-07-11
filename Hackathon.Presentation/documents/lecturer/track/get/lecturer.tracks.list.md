# GET /api/v1/lecturer/events/{eventId}/tracks

> Lecturer lấy danh sách track của 1 event (chỉ IsDisable=false), có phân trang và tìm kiếm.

**Controller:** [LecturerTrackController.cs](Controllers/Lecturer/LecturerTrackController.cs)

## Nghiệp vụ

- Lecturer xem tất cả track của event.
- Chỉ lấy track có IsDisable = false.
- KHÔNG kiểm tra Lecturer có được assign vào event hay không.
- Hỗ trợ tìm kiếm theo keyword (tên track).

## Phân quyền
- ✅ Lecturer (RoleEnum.Lecturer)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| eventId | Guid | ID của event |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Tìm kiếm theo tên track |
| pageIndex | int | No | 1 | Trang số |
| pageSize | int | No | 10 | Số item (max 100) |

## Response (200)
```json
{
  "data": {
    "tracks": [
      {
        "id": "guid",
        "eventId": "guid",
        "title": "AI - Trí tuệ nhân tạo",
        "description": "Mô tả track",
        "maxTeam": 10,
        "isDisable": false,
        "createdAt": "2026-07-11T00:00:00Z",
        "updatedAt": "2026-07-11T00:00:00Z"
      }
    ],
    "totalCount": 5,
    "pageIndex": 1,
    "pageSize": 10
  }
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | You do not have permission | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events/{eventId}/tracks)
