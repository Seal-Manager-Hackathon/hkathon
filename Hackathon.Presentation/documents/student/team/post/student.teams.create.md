# POST /api/v1/student/teams

> Student tạo team mới, user tạo sẽ làm leader.

**Controller:** [StudentTeamController.cs](Controllers/Student/StudentTeamController.cs)

## Nghiệp vụ
- Tạo team với tên team do user nhập.
- Mặc định CanEdit = true (team có thể chỉnh sửa).
- Tự động tạo TeamDetail cho user tạo với IsLeader = true, Status = Active.
- User chỉ có thể tạo team cho chính mình (lấy UserId từ JWT token).
- **Phải điền đủ profile trước khi tạo team:** Email, FirstName, LastName, College, StudentId, PhoneNumber.

## Phân quyền
- ✅ Student

## Request

### Body
```json
{
  "name": "FTeam"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| name | ✅ | Tối đa 200 ký tự |

## Response (201)
```json
{
  "data": {
    "id": "guid",
    "name": "FTeam",
    "canEdit": true
  },
  "message": "Team Created Successfully",
  "status": 201
}
```

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 400 | Name Is Required | Thiếu tên team | Validation form |
| 400 | Please Complete Your Profile Before Proceeding. Missing Fields: ... | Thiếu thông tin profile | Vào trang edit profile |
| 400 | Team Name Already Exists | Tên team đã tồn tại | Đặt tên khác |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Ko phải Student | Ẩn chức năng |
