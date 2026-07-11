# GET /api/v1/lecturer/events/my-lecturer

> Lecturer lấy danh sách event mà họ được phân công (AssignEvents, EventRole = Judge/Mentor), không phân biệt event có bị disable hay không.

**Controller:** [LecturerEventController.cs](Controllers/Lecturer/LecturerEventController.cs)

## Nghiệp vụ

**Router:** `GET /api/v1/lecturer/events/my-lecturer`

- Chỉ lấy các bản ghi AssignEvents mà user hiện tại có EventRole = Judge hoặc Mentor.
- Tự động loại bỏ event có status Draft.
- Trả về tất cả event kể cả event bị disable (IsDisable = true) — FE dùng field isDisable để hiển thị.
- Hỗ trợ lọc theo keyword, status, khoảng thời gian.
- Hỗ trợ phân trang.
- Response giống Admin GET /api/v1/admin/events.

## Phân quyền
- ✅ Lecturer (phải có bản ghi AssignEvents với EventRole = Judge/Mentor)

## Request
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| Keyword | string | ❌ | `Hackathon` | Search tên event |
| Status | string | ❌ | `Published` | ⚠️ Enum: Draft, Published, Closed |
| IsDisable | bool | ❌ | `false` | Lọc theo trạng thái disable |
| FromDate | datetime | ❌ | `2026-07-01T00:00:00Z` | |
| ToDate | datetime | ❌ | `2026-07-07T23:59:59Z` | |
| PageIndex | int | ❌ | `1` | Mặc định 1 |
| PageSize | int | ❌ | `10` | Mặc định 10 |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "guid",
        "name": "Hackathon AI 2026",
        "description": "Cuộc thi về trí tuệ nhân tạo",
        "status": "Published",
        "startTime": "2026-06-01T00:00:00Z",
        "endTime": "2026-07-01T00:00:00Z",
        "isDisable": false,
        "eventRoleId": "guid",
        "eventRoleName": "Mentor",
        "createdAt": "2026-05-01T00:00:00Z",
        "updatedAt": "2026-06-01T00:00:00Z"
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| `eventRoleId` | ID event role của lecturer trong event (VD: Mentor, Judge) |
| `eventRoleName` | Tên event role: `Mentor`, `Judge`, `Staff` |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Invalid Status. Must be: Draft, Published, Closed | Status sai |
| 400 | Page Index Must Be Greater Than Zero | PageIndex < 1 |
| 400 | Page Size Must Be Between 1 And 100 | PageSize ngoài khoảng |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer |

> **Ref:** [Admin API tương ứng](/api/v1/admin/events)
