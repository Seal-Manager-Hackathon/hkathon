# POST /api/v1/admin/assign/event-assigns/{assignEventId}/tracks

> Admin phân công assign event (user đã được gán vào event) vào 1 track.

## Nghiệp vụ
- Gán 1 assign event vào 1 track để user có thể làm việc trong track đó
- Track và assign event phải thuộc cùng 1 event
- Không được gán duplicate (1 assign event chỉ được gán 1 lần vào 1 track)
- Dùng cho Mentor/Judge/Staff — Student không có assign event

## Phân quyền
- ✅ Admin

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| assignEventId | Guid | ID của assign event (bản ghi user đã được gán vào event) |

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
| 400 | Track Does Not Belong To The Same Event | track thuộc event khác với assign event |
| 409 | Track Is Already Assigned To This User | đã có assign track cho cặp này |
| 401 | Unauthorized | Token hết hạn/thiếu |
| 403 | Forbidden | Không phải Admin |

## Logic
1. Kiểm tra assign event tồn tại
2. Kiểm tra track tồn tại
3. Kiểm tra track cùng event với assign event
4. Kiểm tra ko bị duplicate assign
5. Tạo record `AssignTracks` mới
