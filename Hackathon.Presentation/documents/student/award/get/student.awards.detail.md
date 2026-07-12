# GET /api/v1/student/awards/{awardId}

> Student xem chi tiết 1 award (chỉ thấy award active, ko bị disable).

**Controller:** [StudentAwardController.cs](Controllers/Student/StudentAwardController.cs)

> **Ref:** [Admin API tương ứng](/api/v1/admin/awards/{awardId})

## Nghiệp vụ

- Giống Admin API nhưng **chỉ lấy award có IsDisable = false**.
- Nếu award bị disable → 404 Not Found.
- Student ko cần biết eventId để xem detail award.

## Phân quyền

- ✅ Student

## Request

| Param   | Kiểu | Bắt buộc   | Ví dụ                                  |
| ------- | ---- | ---------- | -------------------------------------- |
| awardId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)

```json
{
  "data": {
    "id": "guid",
    "eventId": "guid",
    "name": "Giải Nhất",
    "description": "Trao cho đội có điểm cao nhất",
    "levelAward": 1,
    "numberOfAward": 1,
    "prize": 10000000,
    "isDisable": false,
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi

| Status | message                                           | Khi nào                               | FE xử lý             |
| ------ | ------------------------------------------------- | ------------------------------------- | -------------------- |
| 401    | Invalid Or Expired Token                          | Token hết hạn/thiếu                   | Redirect login       |
| 403    | You do not have permission to perform this action | Ko phải Student                       | Ẩn chức năng         |
| 404    | Resource Not Found                                | awardId ko tồn tại hoặc đã bị disable | Báo "Không tìm thấy" |
