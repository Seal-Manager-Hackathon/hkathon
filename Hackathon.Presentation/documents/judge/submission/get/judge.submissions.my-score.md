# GET /api/v1/judge/submissions/{submissionId}/my-score

> Judge xem diem cham cua chinh minh cho 1 bai nop.

## Nghiep vu

Judge muon xem lai diem minh da cham cho 1 bai nop:
- Tra ve scoreId, assignTrackId (scope), totalScore, va chi tiet tung tieu chi.
- Neu judge chua cham submission nay -> tra ve empty (scoreId = null, scoreItems = []).
- Judge phai duoc assign vao event chua submission.

## Phan quyen
- Judge (RoleEnum = Lecturer), duoc assign vao event

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| submissionId | Guid | ID cua bai nop |

## Response (200) - da cham
```json
{
  "data": {
    "submissionId": "guid",
    "scoreId": "guid",
    "assignTrackId": "guid",
    "totalScore": 85.5,
    "isRetake": false,
    "isMock": false,
    "scoreItems": [
      {
        "criteriaItemId": "guid",
        "criteriaItemName": "Tinh sang tao",
        "score": 8.5,
        "comment": "Tot"
      }
    ]
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Response (200) - chua cham
```json
{
  "data": {
    "submissionId": "guid",
    "scoreId": null,
    "assignTrackId": null,
    "totalScore": null,
    "isRetake": false,
    "isMock": false,
    "scoreItems": []
  },
  "message": "Fetched Successfully",
  "status": 200,
  "traceId": "00-..."
}
```

## Loi
| Status | message | Khi nao |
|--------|---------|---------|
| 401 | Unauthorized | Token het han/thieu |
| 403 | ... | Judge khong duoc assign vao event |
| 404 | Resource Not Found | submissionId khong ton tai |
