# POST /api/v1/admin/register-teams/{registerTeamId}/approve

> Admin đồng ý cho register team tham gia event.

## Nghiệp vụ
- Chỉ approve được register team đang ở trạng thái `Pending`
- Chỉ được approve trong thời gian diễn ra event — không thể approve trước khi event bắt đầu hoặc sau khi event đã kết thúc
- Check event có round No1 không, nếu có check limit team còn slot không
- Nếu còn slot: tự động thêm register team vào round detail
- Kiểm tra từng thành viên trong team không được approved ở team khác trong cùng event
- Set `CanEdit = false` cho team (khóa thành viên)

## Phân quyền
- ✅ Admin

## Response (200)
```json
{
  "data": null,
  "message": "Updated Successfully",
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
| 400 | Only Pending Register Team Can Be Approved | Status không phải Pending | Báo "Chỉ duyệt được đơn đang chờ" |
| 400 | Cannot Approve Before Event Starts | Event chưa bắt đầu | Báo "Sự kiện chưa bắt đầu" |
| 400 | Cannot Approve After Event Has Ended | Event đã kết thúc | Báo "Sự kiện đã kết thúc" |
| 400 | Round 1 Is Full. Cannot Approve More Teams | Hết slot round 1 | Báo "Vòng 1 đã đầy" |
| 400 | One Or More Team Members Are Already Approved In Another Team For This Event | Có thành viên đã được duyệt ở team khác trong event | Báo "Thành viên đã thuộc team khác" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Register Team Not Found | registerTeamId không tồn tại | Báo "Không tìm thấy đơn đăng ký" |
