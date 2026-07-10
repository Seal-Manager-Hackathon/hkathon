# GET /api/v1/staff/events/chapter/{year}/leaderboard

> Staff xem bảng xếp hạng chapter theo năm.

## Nghiệp vụ

- Tính điểm chapter (`chapterScore`) cho từng team dựa trên điểm các event trong năm
- `chapterScore` = AVG(eventScore) của các event team tham gia trong năm đó
- `eventScore` = weighted average (weight=1) của scopeScore các round
- Chỉ tính các event đã Published, team Approved
- Sắp xếp theo chapterScore **từ cao xuống thấp**
- Phân trang

## Phân quyền

- ✅ Staff

## Request

| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| year  | int  | ✅ (route) | `2026` |

| Param     | Kiểu | Bắt buộc | Mô tả |
|-----------|------|----------|-------|
| pageIndex | int  | ❌        | Mặc định 1 |
| pageSize  | int  | ❌        | Mặc định 10, tối đa 100 |

## Response (200)

```json
{
  "data": {
    "year": 2026,
    "eventCount": 3,
    "items": [
      {
        "rank": 1,
        "teamId": "guid",
        "teamName": "Team A",
        "chapterScore": 90.5,
        "eventCount": 2,
        "eventScores": [
          {
            "eventId": "guid",
            "eventName": "Hackathon 2026 - Vòng loại",
            "registerTeamId": "guid",
            "eventScore": 92.0
          },
          {
            "eventId": "guid",
            "eventName": "Hackathon 2026 - Chung kết",
            "registerTeamId": "guid",
            "eventScore": 89.0
          }
        ]
      }
    ],
    "totalCount": 10,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff |

> **Ref:** [Admin API tương ứng](/api/v1/admin/leaderboard/get/admin.leaderboard.chapter.md)