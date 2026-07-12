# Cập nhật lần 11 — Submission status & xoá GradingStatus

## API thay đổi response

### Judge: GET /api/v1/judge/submissions/{submissionId}

**Response cũ:** có field `isGraded`
**Response mới:** xoá `isGraded`. `status` dùng `SubmissionStatusEnum`: `Submitted` = mới nộp, `Graded` = đã chấm.

### Judge: GET /api/v1/judge/rounds/{roundId}/submissions
### Judge: GET /api/v1/judge/tracks/{trackId}/submissions
### Judge: GET /api/v1/judge/events/{eventId}/myscope
### Judge: GET /api/v1/judge/register-teams/{registerTeamId}/submissions

**Response cũ (item):**
```json
{
  "lastSubmission": { "status": "Submitted" },
  "gradingStatus": "Pending",     // computed từ myScore, ko phải submission status
  "scoreId": null,
  "totalScore": null
}
```

**Response mới — xoá `gradingStatus`:**
```json
{
  "lastSubmission": { "status": "Submitted" },  // Submitted / Graded
  "scoreId": null,
  "totalScore": null
}
```

**Field `status` trong `lastSubmission`:**
- `Submitted` — bài mới nộp, chưa được chấm
- `Graded` — đã có judge chấm điểm

### Judge: GET /api/v1/judge/events/{eventId}/scores/me

**Response cũ (item):** có `gradingStatus`
**Response mới:** xoá `gradingStatus`, thêm `status` (SubmissionStatusEnum)

### Judge: GET /api/v1/judge/tracks/{trackId}

**Response cũ:** có `gradedSubmissionCount`
**Response mới:** xoá `gradedSubmissionCount`

## Các API đã check — ko thay đổi

- `GET /api/v1/judge/submissions/{submissionId}/criteria`
- `GET /api/v1/judge/submissions/{submissionId}/my-score`
- `GET /api/v1/lecturer/events/{eventId}/submissions` (đã dùng submission status)
- `GET /api/v1/lecturer/register-teams/{registerTeamId}/submissions` (đã dùng submission status)
- `GET /api/v1/lecturer/tracks/{trackId}/submissions` (đã dùng submission status)
- Tất cả Admin submission APIs (đã dùng submission status)

## Ghi chú cho FE

Các response trước đây có 2 field trùng lặp:
- `lastSubmission.status` — trạng thái thực tế của bài nộp (SubmissionStatusEnum)
- `gradingStatus` — trạng thái chấm của judge hiện tại (dựa trên `myScore != null`)

Giờ chỉ giữ lại `lastSubmission.status`. Filter `status=Graded` / `Pending` vẫn hoạt động như cũ, nhưng dựa trên `submission.Status` thay vì `myScore`.
