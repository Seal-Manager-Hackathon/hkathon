# GET /api/v1/mentor/tracks/{trackId}/register-teams

> Mentor (Lecturer có EventRole = Mentor) lấy danh sách register teams của 1 track có phân trang, search theo tên team.
> **Controller:** `MentorRegisterTeamController` — `GET /api/v1/mentor/tracks/{trackId}/register-teams?keyword=&pageIndex=1&pageSize=10`

## Nghiệp vụ

- Mentor muốn xem tất cả các đội đã đăng ký (register teams) trong 1 track mà họ quan tâm.
- **Chỉ lấy register teams có `IsDisable = false`** — các đội bị disable sẽ không xuất hiện.
- Có phân trang: mặc định page 1, pageSize 10.
- Hỗ trợ tìm kiếm theo tên đội (`keyword`), không phân biệt hoa thường, tìm chuỗi con.
- Sắp xếp theo `CreatedAt` giảm dần (mới nhất lên đầu).

## Phân quyền
- ✅ Mentor (RoleEnum = Lecturer + EventRole = Mentor)
- Không cần check lecturer có được assign vào track hay không.

## Request

### Route Parameters
| Parameter | Type | Bắt buộc | Ví dụ | Ghi chú |
|-----------|------|----------|-------|---------|
| trackId | Guid | Có | 3fa85f64-5717-4562-b3fc-2c963f66afa6 | ID của track |

### Query Parameters
| Parameter | Type | Bắt buộc | Mặc định | Ghi chú |
|-----------|------|----------|---------|---------|
| keyword | string | Không | - | Tìm kiếm theo tên team (không phân biệt hoa thường) |
| pageIndex | int | Không | 1 | Trang hiện tại |
| pageSize | int | Không | 10 | Số item mỗi trang |

## Response (200)
```json
{
  "data": {
    "registerTeams": [
      {
        "registerTeamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "teamId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "teamName": "FTeam",
        "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "trackName": "Tri tue nhan tao",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicName": "AI trong y te",
        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "eventName": "Hackathon 2026",
        "description": "Team đăng ký tham gia",
        "status": "Approved",
        "isBanned": false,
        "isDisable": false,
        "roundId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "roundName": "Vòng 1",
        "roundNo": 1
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "..."
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| registerTeamId | ID của register team |
| teamId | ID của đội |
| teamName | Tên đội |
| trackId | ID của track |
| trackName | Tên track |
| topicId | ID của topic (có thể null) |
| topicName | Tên topic (có thể null) |
| eventId | ID của event |
| eventName | Tên event |
| description | Mô tả đăng ký |
| status | Trạng thái: Pending, Approved, Rejected, Banned |
| isBanned | Có bị cấm không |
| isDisable | Có bị vô hiệu hóa không (luôn false) |
| roundId | ID của round hiện tại |
| roundName | Tên round hiện tại |
| roundNo | Số thứ tự round hiện tại |
| totalCount | Tổng số bản ghi |
| pageIndex | Trang hiện tại |
| pageSize | Số item mỗi trang |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | PageIndex invalid | pageIndex không hợp lệ |
| 400 | PageSize invalid | pageSize không hợp lệ |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Lecturer (mentor) |

> **Ref:** API này là bản gốc cho Mentor, không có Admin/Lecturer tương ứng.
