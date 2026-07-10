# GET /api/v1/lecturer/events

> Lecturer đã đăng nhập lấy danh sách các event mà họ được phân công (với vai trò Judge hoặc Mentor), có filter và phân trang. Response giống hệt Admin GET /api/v1/admin/events.

## Nghiệp vụ

- Lecturer muốn xem tất cả event họ được gán làm việc (qua bảng AssignEvents với event role là Judge hoặc Mentor).
- API này giúp lecturer nhanh chóng tìm event cần làm việc.
- Mặc định loại trừ event có status Draft.
- Hỗ trợ tìm kiếm theo từ khóa (tên event), lọc theo status, và khoảng thời gian.
- Kết quả sắp xếp theo ngày tạo event mới nhất trước.
- Response fields giống hệt Admin Event list (EventItem: id, name, description, status, startTime, endTime, isDisable, createdAt, updatedAt).

## Phân quyền
- ✅ Lecturer (RoleEnum = Lecturer) — phải được phân công vào ít nhất 1 event (Judge/Mentor)

## Request

### Query Parameters
| Parameter | Type | Bắt buộc | Mặc định | Ví dụ | Ghi chú |
|-----------|------|----------|---------|-------|---------|
| Keyword | string | Không | - | Hackathon | Tìm kiếm theo tên event |
| Status | string | Không | - | Published | Lọc theo status: `Draft`, `Published`, `Closed` |
| FromDate | DateTimeOffset | Không | - | 2026-01-01 | Lọc từ ngày (theo CreatedAt) |
| ToDate | DateTimeOffset | Không | - | 2026-12-31 | Lọc đến ngày (theo CreatedAt) |
| PageIndex | int | Không | 1 | 1 | Trang hiện tại |
| PageSize | int | Không | 10 | 10 | Số lượng item mỗi trang (max 100) |

## Response (200)
```json
{
  "data": {
    "events": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Hackathon AI 2026",
        "description": "Cuộc thi trí tuệ nhân tạo",
        "status": "Published",
        "startTime": "2026-07-01T00:00:00Z",
        "endTime": "2026-07-10T00:00:00Z",
        "isDisable": false,
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
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-07T12:00:00Z"
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | PageIndex/PageSize invalid | PageIndex hoặc PageSize không hợp lệ | Hiển thị thông báo lỗi |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Chuyển về trang login |
| 403 | You do not have permission to perform this action | Không phải Lecturer | Hiển thị thông báo Không có quyền |
