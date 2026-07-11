# GET /api/v1/judge/submissions/{submissionId}/criteria

> Judge lấy criteria template active của round (kèm danh sách tiêu chí), format giống Admin `GET /api/v1/admin/criteria-templates/{templateId}`.

**Controller:** [JudgeController.cs](Controllers/Judge/JudgeController.cs)

## Nghiệp vụ

- Dùng active criteria template của round (submission → round → active template).
- Response format giống Admin `GET /api/v1/admin/criteria-templates/{templateId}`.
- Judge phải được assign vào track của submission.

## Phân quyền
- ✅ Judge — phải được assign vào track

## Request

### Route Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| submissionId | Guid | ID của submission |

## Response (200)
```json
{
  "data": {
    "submissionId": "guid",
    "id": "guid",
    "roundId": "guid",
    "title": "Template đánh giá vòng 1",
    "description": "Các tiêu chí chấm điểm",
    "isDisable": false,
    "isActive": true,
    "items": [
      {
        "id": "guid",
        "criteriaTemplateId": "guid",
        "name": "Tính sáng tạo",
        "description": "Mức độ sáng tạo của ý tưởng",
        "score": 30,
        "isDisable": false,
        "createdAt": "2026-07-07T12:00:00Z"
      }
    ],
    "createdAt": "2026-07-07T12:00:00Z",
    "updatedAt": "2026-07-07T12:00:00Z"
  },
  "message": "Fetched Successfully",
  "error": null,
  "isSuccess": true,
  "status": 200,
  "traceId": "00-abc123...",
  "timestampUtc": "2026-07-11T12:00:00Z"
}
```

### Field ý nghĩa
| Field | Ý nghĩa |
|-------|---------|
| `submissionId` | ID của submission (context) |
| `id` | ID của criteria template |
| `roundId` | Round của template |
| `title` / `description` | Tên và mô tả template |
| `isDisable` / `isActive` | Trạng thái template |
| `items` | Danh sách criteria items |
| `items[].id` | ID của criteria item |
| `items[].criteriaTemplateId` | ID của template cha |
| `items[].name` | Tên tiêu chí |
| `items[].description` | Mô tả tiêu chí |
| `items[].score` | Điểm tối đa của tiêu chí |
| `items[].isDisable` | Tiêu chí còn hoạt động? |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Or Expired Token | Token hết hạn |
| 403 | You Are Not Assigned as Judge for This Track | Judge ko được assign |
| 404 | Submission Not Found | submissionId ko tồn tại |

> **Ref:** [Admin API tương ứng](/api/v1/admin/criteria-templates/{templateId})
