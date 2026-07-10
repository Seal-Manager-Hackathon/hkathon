# POST /api/v1/staff/event-assigns/{assignEventId}/tracks

> Staff phân công một Lecturer (đã được gán vào event) vào 1 track.

## Nghiệp vụ
- Gán 1 Lecturer vào 1 track để Lecturer có thể làm việc trong track đó (chấm điểm, mentor)
- Track và assign event phải thuộc cùng 1 event
- Không được gán duplicate (1 assign event chỉ được gán 1 lần vào 1 track)
- **Chỉ dành cho Lecturer** — Staff không thể assign track cho Staff

## Phân quyền
- ✅ Staff (phải được phân công vào event tương ứng)

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của assign event (bản ghi Lecturer đã được gán vào event) |

### Body (JSON)
```json
{
  "trackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| trackId | ✅ | Phải tồn tại và cùng event với assign event |

## Response (201)
```json
{
  "data": null,
  "message": "Created Successfully",
  "status": 201,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 404 | Assign Event Not Found | assignEventId ko tồn tại |
| 404 | Track Not Found | trackId ko tồn tại |
| 400 | Can Only Assign Track To Lecturer | User không phải Lecturer |
| 400 | Track Does Not Belong To The Same Event | track thuộc event khác với assign event |
| 409 | Track Is Already Assigned To This User | đã có assign track cho cặp này |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Staff / không được assign vào event |

## Logic
1. Kiểm tra assign event tồn tại và user là Lecturer
2. Kiểm tra track tồn tại
3. Kiểm tra track cùng event với assign event
4. Kiểm tra ko bị duplicate assign
5. Tạo record `AssignTracks` mới

> **Ref:** [Admin API tương ứng](/api/v1/admin/assign/event-assigns/{assignEventId}/tracks) — [`admin/assign/post/admin.assign.tracks.assign.md`](../../../admin/assign/post/admin.assign.tracks.assign.md)
