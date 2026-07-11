# Update 09 — 11/07/2026

## 1. Sửa route Judge (theo pattern cũ)
**Trước:**
- `POST /api/v1/judge/submissions/{submissionId}/score`
- `GET /api/v1/judge/submissions/{submissionId}/my-score`
- `GET /api/v1/judge/events/{eventId}/my-scores`

**Sau:** (đồng bộ với `Hackathon.Service.Judges` cũ)
- `POST /api/v1/judge/submissions/{submissionId}/scores`
- `GET /api/v1/judge/submissions/{submissionId}/scores/me`
- `GET /api/v1/judge/scores/me?eventId=...` (eventId chuyển sang query param)

**Files:** `Controllers/Judge/JudgeController.cs`

## 2. Sửa route Admin Award (bỏ eventId)
**Trước:** (4 routes đều có `events/{eventId}/awards/{awardId}`)
- `PATCH /api/v1/admin/events/{eventId}/awards/{awardId}`
- `POST /api/v1/admin/events/{eventId}/awards/{awardId}/swap`
- `POST /api/v1/admin/events/{eventId}/awards/{awardId}/restore`
- `POST /api/v1/admin/events/{eventId}/awards/{awardId}/delete`

**Sau:** (bỏ eventId, award tự biết event của nó)
- `PATCH /api/v1/admin/awards/{awardId}`
- `POST /api/v1/admin/awards/{awardId}/swap`
- `POST /api/v1/admin/awards/{awardId}/restore`
- `POST /api/v1/admin/awards/{awardId}/delete`

## 4. Sửa Judge SubmitScoreRequest — bỏ TotalScore, tự tính SUM
**Trước:** `SubmitScoreRequest` yêu cầu FE gửi `totalScore`, service override `score.TotalScore = request.TotalScore`  
**Sau:** Bỏ `TotalScore` khỏi request, `ScoreSubmissionHelper.CreateScore` tự tính = SUM ScoreItems. `UpdateScore` cũng tự tính total = SUM.  
**Files:** `Services/Judge/Request.cs`, `Services/Judge/Service.cs`
**Docs:** `documents/judge/score/post/judge.scores.submit.md`

**Files:** `Controllers/Admin/AdminAwardController.cs`
**Docs:** `documents/admin/award/**/*.md` (patch + swap + delete + restore)

## 3. Sửa route Admin Round swap (bỏ eventId)
**Trước:** `POST /api/v1/admin/events/{eventId}/rounds/{roundId}/swap`  
**Sau:** `POST /api/v1/admin/rounds/{roundId}/swap`  
**Files:** `Controllers/Admin/AdminRoundController.cs`, `Services/Admin/Round/Service.cs`
**Docs:** `documents/admin/round/post/admin.rounds.swap.md`
