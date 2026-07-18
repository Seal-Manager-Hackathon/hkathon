# Judge — Notification Mapping
> Controller: JudgeController

## Scoring
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | submissions/{submissionId}/scores | SubmitScore | Your submission for round {RoundNo} has been scored | Personal → all team members |

> **Note:** Judge controller dùng chung với Lecturer (lecturer có thể mang role judge).
