# GET /api/v1/staff/score-items/{scoreItemId}

> Xem chi tiết một score item (điểm của một criteria).

## Nghiệp vụ
- Staff chỉ xem được score item thuộc event mình được phân công
- Trả về criteria, điểm, comment, grader, track/topic

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Param | Kiểu | Bắt buộc | Ví dụ | Ghi chú |
|-------|------|----------|-------|---------|
| scoreItemId | Guid | Có | `3fa85f64-5717-4562-b3fc-2c963f66afa6` | ID của score item |

## Response (200)
Giống item trong `GET /api/v1/staff/scores/{scoreId}/items`.

## Lỗi
| Status | message | Khi nào | FE xử lý |
|--------|---------|---------|----------|
| 401 | Invalid Or Expired Token | Token hết hạn/thiếu | Redirect login |
| 403 | You do not have permission to perform this action | Không phải Staff hoặc không được phân công vào event | Ẩn chức năng |
| 404 | Resource Not Found | scoreItemId không tồn tại | Hiển thị thông báo |

> **Ref:** [Admin API tương ứng](/api/v1/admin/score-items/{scoreItemId}) — [`admin/score/get/admin.score-items.detail.md`](../../../admin/score/get/admin.score-items.detail.md)
