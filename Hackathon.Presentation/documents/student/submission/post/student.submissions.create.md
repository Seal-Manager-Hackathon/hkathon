# POST /api/v1/student/submissions

> Student (team leader) nộp bài cho 1 round của event.

**Controller:** [StudentSubmissionController.cs](../../../Controllers/Student/StudentSubmissionController.cs)

## Nghiệp vụ

Team leader muốn nộp bài cho team trong 1 round:
- Chỉ **team leader** mới được nộp bài.
- **registerTeamId** và **roundId** bắt buộc.
- Round phải thuộc cùng event với register team.
- Team phải được đăng ký vào round đó (có RoundDetail).
- **Kiểm tra thời gian nộp bài:**
  - `StartSubmission`: phải đã tới thời gian bắt đầu nộp.
  - `EndSubmission`: phải chưa quá thời gian kết thúc nộp.
- Mỗi lần nộp tạo 1 record mới (không ghi đè).
- Các API khác luôn lấy bài nộp **cuối cùng** của mỗi round.
- Status mặc định: `Submitted`.

## Phân quyền
- ✅ Student (phải là leader của team)

## Request

### Body
```json
{
  "registerTeamId": "guid",
  "roundId": "guid",
  "url": "https://example.com/submission.pdf",
  "description": "Bài nộp vòng 1"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| registerTeamId | ✅ | ID đăng ký team trong event |
| roundId | ✅ | ID round muốn nộp |
| url | ✅ | Link bài nộp |
| description | ❌ | Mô tả thêm |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "registerTeamId": "guid",
    "roundId": "guid",
    "url": "https://example.com/submission.pdf",
    "description": "Bài nộp vòng 1",
    "status": "Submitted",
    "submittedAt": "2026-07-13T10:00:00Z"
  },
  "message": "Submitted Successfully",
  "status": 201,
  "traceId": "00-..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 400 | Only Team Leader Can Submit | User không phải leader |
| 400 | Round Does Not Belong to This Event | Round khác event với register team |
| 400 | Team Is Not Registered in This Round | Team chưa được add vào round |
| 400 | Submission Period Has Not Started Yet | Chưa tới StartSubmission |
| 400 | Submission Period Has Ended | Đã qua EndSubmission |
| 400 | RegisterTeamId/RoundId/Url Is Required | Thiếu field bắt buộc |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 404 | Register Team Not Found | registerTeamId ko tồn tại/bị disable |
| 404 | Round Not Found | roundId ko tồn tại/bị disable |
