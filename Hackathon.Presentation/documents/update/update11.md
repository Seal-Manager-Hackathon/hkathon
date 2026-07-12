# Cập nhật lần 11 — Submission status & xoá GradingStatus

## Lý do

Trước đây chưa có enum `SubmissionStatusEnum` trong entity `Submissions`, nên phải query score để biết đã chấm chưa (dùng `myScore != null` → `"Graded"/"Pending"`). Giờ `Submissions` đã có `Status` (Submitted / Graded / Failed), nên đơn giản hoá:

- **Response:** xoá `gradingStatus`, `isGraded`, `gradedSubmissionCount`. Dùng `status` từ `SubmissionStatusEnum`.
- **Logic khi chấm:** set `submission.Status = Graded` trong `SubmitScore` và `UpdateScore`.
- **Filter:** `isGraded` query param vẫn giữ nguyên tên, nhưng filter dựa trên `submission.Status` thay vì `myScore`.

---

## API thay đổi response

### 1. GET /api/v1/judge/submissions/{submissionId}

**Cũ:**
```json
{
  "status": "Submitted",
  "isGraded": false,    ← xoá
  "totalScore": 170.50,
  "judgeCount": 2
}
```

**Mới:** xoá `isGraded`.

### 2. GET /api/v1/judge/tracks/{trackId}/submissions

**Cũ:**
```json
{
  "lastSubmission": { "status": "Submitted" },
  "gradingStatus": "Pending",   ← xoá
  "scoreId": null
}
```

**Mới:** xoá `gradingStatus`.

### 3. GET /api/v1/judge/rounds/{roundId}/submissions

**Cũ:** có `gradingStatus` trong item.
**Mới:** xoá `gradingStatus`.

### 4. GET /api/v1/judge/events/{eventId}/myscope

**Cũ:** có `gradingStatus` trong item.
**Mới:** xoá `gradingStatus`.

### 5. GET /api/v1/judge/register-teams/{registerTeamId}/submissions

**Cũ:**
```json
{
  "lastSubmission": { "status": "Submitted" },
  "gradingStatus": "Graded",   ← xoá
  "scoreId": "guid"
}
```

**Mới:** xoá `gradingStatus`.

### 6. GET /api/v1/judge/events/{eventId}/scores/me

**Cũ:**
```json
{
  "submissionId": "guid",
  "gradingStatus": "Graded",   ← xoá
  "scoreId": "guid"
}
```

**Mới:** thay `gradingStatus` bằng `status`: `"Submitted"` / `"Graded"`.

### 7. GET /api/v1/judge/tracks/{trackId}

**Cũ:** có `gradedSubmissionCount`.
**Mới:** xoá `gradedSubmissionCount`.

---

## Thay đổi logic

| Phương thức | Thay đổi |
|------------|----------|
| `SubmitScore` | Thêm `submission.Status = SubmissionStatusEnum.Graded` + `submission.UpdatedAt` |
| `UpdateScore` | Thêm `score.Submission.Status = SubmissionStatusEnum.Graded` + `score.Submission.UpdatedAt` |
| Tất cả filter `isGraded` | Filter dựa trên `lastSubmission.Status == "Graded"` thay vì `myScore != null` |

---

## File thay đổi

### Code
| File | Thay đổi |
|------|----------|
| `Judge/Response.cs` | Xoá `GradingStatus` khỏi `TrackSubmissionItem`, `GetRegisterTeamSubmissionsResponse`, `JudgeMyScoreItem`; xoá `GradedSubmissionCount` khỏi `JudgeTrackItem` |
| `Judge/Service.cs` | Xoá toàn bộ logic `GradingStatus`, thay filter bằng `submission.Status`; thêm set `Status = Graded` trong `SubmitScore` + `UpdateScore` |

### Doc
| File | Thay đổi |
|------|----------|
| `judge.submissions.detail.md` | Xoá `isGraded` |
| `judge.submissions.by-track.md` | Xoá `gradingStatus` khỏi JSON + field table |
| `judge.submissions.by-round.md` | Xoá `gradingStatus` khỏi JSON + field table |
| `judge.submissions.myscope.md` | Xoá `gradingStatus` khỏi JSON, sửa filter description |
| `judge.register-teams.submissions.md` | Xoá `gradingStatus` khỏi JSON + field table |
| `judge.scores.my-scores.md` | Thay `gradingStatus` bằng `status` trong JSON + field table |

---

## API không thay đổi

- Tất cả Admin submission APIs (đã dùng `submission.Status` từ đầu)
- Tất cả Lecturer submission APIs (đã dùng `submission.Status` từ đầu)
- `GET /api/v1/judge/submissions/{submissionId}/criteria`
- `GET /api/v1/judge/submissions/{submissionId}/my-score`
