# POST /api/v1/staff/register-teams/{registerTeamId}/approve — Duyệt team

## Mục đích

Staff duyệt (approve) 1 team đã đăng ký tham gia event. Chỉ duyệt được team đang ở trạng thái `Pending`.

## Business Logic

1. Team phải có status = `Pending`
2. Kiểm tra round 1 còn slot không (nếu limit < current count → báo lỗi)
3. Set status = `Approved`, khóa team (`CanEdit = false`)
4. Tự động tạo RoundDetail cho round đầu tiên

## Endpoint

```
POST /api/v1/staff/register-teams/{registerTeamId}/approve
```