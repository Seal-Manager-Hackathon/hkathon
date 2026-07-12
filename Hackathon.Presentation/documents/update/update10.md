# Update 10 — Sửa router Award

## Tất cả API Admin Award

| # | Method | Route cũ | Route mới | Thay đổi |
|---|--------|----------|-----------|----------|
| 1 | GET | `/api/v1/admin/events/{eventId}/awards` | giữ nguyên | eventId route param → query param? ❌ **Giữ nguyên** |
| 2 | POST | `/api/v1/admin/events/{eventId}/awards` | giữ nguyên | eventId route param → body? ❌ **Giữ nguyên** |
| 3 | GET | `/api/v1/admin/events/{eventId}/awards/{awardId}` | `/api/v1/admin/awards/{awardId}` | Bỏ `events/{eventId}` |
| 4 | PATCH | `/api/v1/admin/events/{eventId}/awards/{awardId}` | `/api/v1/admin/awards/{awardId}` | Bỏ `events/{eventId}` |
| 5 | POST | `/api/v1/admin/events/{eventId}/awards/{awardId}/delete` | `/api/v1/admin/awards/{awardId}/delete` | Bỏ `events/{eventId}` |

