# GET /api/v1/judge/rounds/{roundId}/submissions

> Judge da dang nhap lay danh sach bai nop cua cac doi trong 1 round ma ho duoc phan cong cham, co loc theo track va phan trang.

## Nghiep vu

Judge muon xem tat ca bai nop trong 1 round cu the:
- Neu **khong truyen trackId**: lay bai nop cua tat ca track judge duoc phan cong trong round do.
- Neu **co truyen trackId**: chi lay bai nop cua track do. Neu judge khong duoc phan cong track nay -> 403 Forbidden.
- Moi register team chi lay bai nop **cuoi cung** (moi nhat) trong round.
- Co the loc theo isGraded (da cham/chua cham dua tren submission status).
- Chi lay cac doi da co bai nop (submissions.Count > 0).
- Sap xep theo thoi gian nop bai giam dan (moi nhat truoc).

## Phan quyen
- ✅ Judge (RoleEnum = Lecturer, duoc assign lam Judge trong event)
- Phai duoc assign vao event chua round do
- Neu co trackId: phai duoc assign vao track do

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| roundId | Guid | ID cua round |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| trackId | Guid | No | - | Loc theo track. Neu de trong -> lay het track duoc phan cong |
| isGraded | bool | No | - | Lọc theo trạng thái chấm: true=đã chấm, false=chưa chấm |
| pageIndex | int | No | 1 | Trang hien tai |
| pageSize | int | No | 10 | So luong item moi trang |

## Response (200)
```json
{
  "data": {
    "items": [
      {
        "registerTeamId": "guid",
        "teamId": "guid",
        "teamName": "Team ABC",
        "eventId": "guid",
        "eventName": "Hackathon 2026",
        "roundId": "guid",
        "roundName": "Vong 1",
        "trackId": "guid",
        "trackTitle": "Tri tue nhan tao",
        "topicId": "guid",
        "topicTitle": "AI trong Y te",
        "submittedBy": {
          "userId": "guid",
          "email": "leader@email.com",
          "firstName": "Nguyen",
          "lastName": "Van A"
        },
        "lastSubmission": {
          "id": "guid",
          "submittedAt": "2026-07-11T10:00:00Z",
          "url": "https://example.com/submission.pdf",
          "description": "Bai nop cuoi",
          "status": "Submitted"
        },
        "scoreId": null,
        "totalScore": null
      }
    ],
    "totalCount": 1,
    "pageIndex": 1,
    "pageSize": 10
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

### Field y nghia

| Field | Y nghia |
|-------|---------|
| items[].registerTeamId | ID dang ky doi vao event |
| items[].teamName | Ten doi |
| items[].lastSubmission.status | "Submitted" (moi nop) / "Graded" (da cham) |
| items[].scoreId | ID cua luot cham (null neu chua cham) |
| items[].totalScore | Tong diem (null neu chua cham) |

## Loi
| Status | message | Khi nao |
|--------|---------|---------|
| 401 | Unauthorized | Token het han/thieu |
| 403 | You Are Not Assigned as Judge for This Track | TrackId khong hop le hoac judge khong duoc phan cong track do |
| 404 | Round Not Found | roundId khong ton tai hoac da bi disable |
| 404 | Event Not Found or You Are Not Assigned to This Event | Judge khong duoc phan cong trong event |