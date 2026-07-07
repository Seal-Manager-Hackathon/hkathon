# Update 02 — Thêm filter IsDisable cho GET list APIs

> Bổ sung field `IsDisable` vào request của các API GET danh sách để FE có thể lọc theo trạng thái disable.

## Quy tắc chung

- Tất cả GET list API đã có `IsDisable` (bool?) trong request để FE truyền lên:
  - Không truyền → lấy tất cả
  - `isDisable=true` → chỉ lấy đã xóa mềm
  - `isDisable=false` → chỉ lấy chưa xóa
- Các API có entity cha (Event, Track, Round) không check IsDisable của cha — admin thấy được tất cả

## Chi tiết thay đổi

### 1. Notifications — GET /api/v1/admin/notifications

| Item | Cũ | Mới |
|------|-----|-----|
| Request | Không có IsDisable | Thêm `bool? IsDisable` |
| Repository.SearchAsync | Không filter IsDisable | Thêm tham số + filter |

**Request cũ:**
```json
{ "title": "...", "targetType": "...", "fromDate": "...", "toDate": "...", "pageIndex": 1, "pageSize": 10 }
```
**Request mới:**
```json
{ "title": "...", "targetType": "...", "fromDate": "...", "toDate": "...", "isDisable": false, "pageIndex": 1, "pageSize": 10 }
```

### 2. Events — GET /api/v1/admin/events

| Item | Cũ | Mới |
|------|-----|-----|
| Request | Không có IsDisable | Thêm `bool? IsDisable` |

**Request cũ:**
```json
{ "keyword": "...", "status": "...", "fromDate": "...", "toDate": "...", "pageIndex": 1, "pageSize": 10 }
```
**Request mới:**
```json
{ "keyword": "...", "status": "...", "isDisable": false, "fromDate": "...", "toDate": "...", "pageIndex": 1, "pageSize": 10 }
```

### 3. Rounds — GET /api/v1/admin/events/{eventId}/rounds

| Item | Cũ | Mới |
|------|-----|-----|
| Request | Không có IsDisable | Thêm `bool? IsDisable` |

**Request cũ:**
```json
{ "keyword": "...", "roundNo": 1, "pageIndex": 1, "pageSize": 10 }
```
**Request mới:**
```json
{ "keyword": "...", "roundNo": 1, "isDisable": false, "pageIndex": 1, "pageSize": 10 }
```

### 4. Teams — GET /api/v1/admin/teams

| Item | Cũ | Mới |
|------|-----|-----|
| Request | Không có IsDisable | Thêm `bool? IsDisable` |

**Request cũ:**
```json
{ "keyword": "...", "canEdit": true, "fromDate": "...", "toDate": "...", "pageIndex": 1, "pageSize": 10 }
```
**Request mới:**
```json
{ "keyword": "...", "canEdit": true, "isDisable": false, "fromDate": "...", "toDate": "...", "pageIndex": 1, "pageSize": 10 }
```

### 5. Register Teams by Team — GET /api/v1/admin/teams/{teamId}/register-teams

| Item | Cũ | Mới |
|------|-----|-----|
| Request | Không có IsDisable | Thêm `bool? IsDisable` |

**Request cũ:**
```json
{ "status": "Approved", "pageIndex": 1, "pageSize": 10 }
```
**Request mới:**
```json
{ "status": "Approved", "isDisable": false, "pageIndex": 1, "pageSize": 10 }
```

## Soft Delete — không check trạng thái cũ

Các service sau đã bỏ check "already disabled / not disabled" — admin gọi delete/restore lúc nào cũng được:
- Team (bỏ 2 check: IsAlreadyDisabled, IsNotDisabled)
- Notification (bỏ 2 check: IsAlreadyDisabled, IsNotDisabled)
- RegisterTeam ban/unban (bỏ 2 check: IsAlreadyBanned, IsNotBanned)

## Các API đã có IsDisable từ trước (không thay đổi)

- `tracks` GET list — có `isDisable`
- `topics` GET list — có `isDisable`
- `criteria-templates` GET list — có `isDisable`
- `criteria-items` GET list — có `isDisable`
- `register-teams` GET list (theo event) — có `isDisable`
- `users/{userId}/teams` GET list — có `isDisable`
