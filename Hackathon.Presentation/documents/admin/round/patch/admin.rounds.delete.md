# PATCH /api/v1/admin/rounds/{roundId}/delete

> Admin xóa soft round khỏi event. RoundNo của round bị xóa set thành 0, các roundNo khác được dồn lại.

## Nghiệp vụ

**Khi xóa 1 round:**
1. `RoundNo` của round bị xóa → **set thành `0`** (không xóa khỏi DB)
2. `NumberRound` của event → **trừ đi 1**
3. Các round có `RoundNo > roundNo cũ` → **RoundNo giảm 1** (dồn lại, không bị ngắt quãng)

**Ví dụ:** Event có 4 rounds (RoundNo 1,2,3,4). Xóa round No=2:
- Round bị xóa: RoundNo từ 2 → 0
- Round No=3 cũ → RoundNo=2
- Round No=4 cũ → RoundNo=3
- NumberRound từ 4 → 3

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
  "message": "Deleted Successfully",
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
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Admin | Ẩn chức năng |
| 404 | Round Not Found | roundId không tồn tại | Báo "Không tìm thấy round" |
