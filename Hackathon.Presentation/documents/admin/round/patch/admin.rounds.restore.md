# PATCH /api/v1/admin/rounds/{roundId}/restore

> Admin khôi phục round đã bị xóa. Các thông tin thời gian bị xóa, RoundNo được cấp mới.

## Nghiệp vụ

**Chỉ cho phép restore round đã bị xóa** (RoundNo = 0). Nếu round chưa bị xóa → báo lỗi.

**Khi restore:**
1. ✅ **RoundNo mới** = `maxRoundNo hiện tại + 1` — xếp cuối cùng
2. ❌ **StartTime → null** — bị xóa, admin phải cập nhật lại sau
3. ❌ **EndTime → null** — bị xóa
4. ❌ **StartSubmission → null** — bị xóa
5. ❌ **EndSubmission → null** — bị xóa
6. ✅ **Các field khác giữ nguyên:** Name, Description, LimitTeam, EventId
7. ✅ **NumberRound của event** → +1

**Luồng điển hình:**
```
1. Delete round → RoundNo=0, các round sau dồn lên
2. Restore round → RoundNo = max+1 (xếp cuối), thời gian null
3. Dùng PUT rounds/{roundId} để cập nhật thời gian mới cho round
```

## Phân quyền
- ✅ Admin

## Request
| Param | Kiểu | Bắt buộc | Ví dụ |
|-------|------|----------|-------|
| roundId | guid | ✅ (route) | `3fa85f64-5717-4562-b3fc-2c963f66afa6` |

## Response (200)
```json
{
  "data": null,
  "message": "Operation Successful",
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
| 400 | Round Is Not Deleted | round chưa bị xóa (RoundNo ≠ 0) | Báo "Round chưa bị xóa" |
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Round Not Found | roundId không tồn tại | Báo "Không tìm thấy round" |
