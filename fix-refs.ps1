# Fix Ref Links in Staff/Lecturer/Base Docs
# User request: "sử dụng gitnexus để quét toàn bộ doc và api nhé, và gán vào doc cái link để ấn 1 cái để qua cái api đó cho dễ tìm"
#
# This script fixes ~50 Staff doc files that have wrong ref links.
# Current: > **Ref:** [Admin API tương ứng](/api/v1/admin/some/doc/path.md) (doc file path)
# Target:  > **Ref:** [Admin API tương ứng](/api/v1/admin/actual/api/route) — [`admin/...doc...md`](../../../admin/.../doc.md) (API route + doc path)

$base = "c:\Users\phamq\OneDrive\Desktop\New folder (4)\Hackathon\Hackathon.Presentation\documents"
$fixed = 0

function Fix-File($path, $oldText, $newText) {
    $abs = Join-Path $base $path
    if (!(Test-Path $abs)) { Write-Host "  NOT FOUND: $path" -F Red; return }
    $content = Get-Content $abs -Raw
    if ($content.Contains($oldText)) {
        $content = $content.Replace($oldText, $newText)
        Set-Content $abs $content -NoNewline -Encoding UTF8
        Write-Host "  ✓ $path" -F Green
        $script:fixed++
    } else { Write-Host "  ? No match in $path" -F Yellow }
}

function Add-First($path) {
    $abs = Join-Path $base $path
    if (!(Test-Path $abs)) { Write-Host "  NOT FOUND: $path" -F Red; return }
    $content = Get-Content $abs -Raw
    if ($content.Contains("**Ref:") -or $content.Contains("**First")) { Write-Host "  ~ Skip $path" -F DarkYellow; return }
    if ($content -match "## Lỗi") {
        $content = $content -replace "## Lỗi", "`n> **First** — API này là bản gốc, không có Admin tương ứng. Dùng làm chuẩn tham chiếu cho các role khác.`n`n## Lỗi"
    }
    Set-Content $abs $content -NoNewline -Encoding UTF8
    Write-Host "  + $path (First)" -F Green
    $script:fixed++
}

# ===== STAFF ASSIGN =====
Fix-File "staff\assign\get\staff.assign.assigned.md" `
    "/api/v1/admin/assign/get/admin.assign.assigned.md" `
    "/api/v1/admin/assign/events/{eventId}/assigned) — [`admin/assign/get/admin.assign.assigned.md`](../../../admin/assign/get/admin.assign.assigned.md"
Fix-File "staff\assign\get\staff.assign.lecturers.available.md" `
    "/api/v1/admin/assign/get/admin.assign.lecturers.available.md" `
    "/api/v1/admin/assign/events/{eventId}/lecturers/available) — [`admin/assign/get/admin.assign.lecturers.available.md`](../../../admin/assign/get/admin.assign.lecturers.available.md"
Fix-File "staff\assign\post\staff.assign.assign-user.md" `
    "/api/v1/admin/assign/post/admin.assign.assign-user.md" `
    "/api/v1/admin/assign/events/{eventId}/assign/users) — [`admin/assign/post/admin.assign.assign-user.md`](../../../admin/assign/post/admin.assign.assign-user.md"
Fix-File "staff\assign\post\staff.assign.event-remove.md" `
    "/api/v1/admin/assign/post/admin.assign.event-remove.md" `
    "/api/v1/admin/assign/event-assigns/{assignEventId}/remove) — [`admin/assign/post/admin.assign.event-remove.md`](../../../admin/assign/post/admin.assign.event-remove.md"
Fix-File "staff\assign\post\staff.assign.event-restore.md" `
    "/api/v1/admin/assign/post/admin.assign.event-restore.md" `
    "/api/v1/admin/assign/event-assigns/{assignEventId}/restore) — [`admin/assign/post/admin.assign.event-restore.md`](../../../admin/assign/post/admin.assign.event-restore.md"
Fix-File "staff\assign\post\staff.assign.tracks.assign.md" `
    "/api/v1/admin/assign/post/admin.assign.tracks.assign.md" `
    "/api/v1/admin/assign/event-assigns/{assignEventId}/tracks) — [`admin/assign/post/admin.assign.tracks.assign.md`](../../../admin/assign/post/admin.assign.tracks.assign.md"
Fix-File "staff\assign\post\staff.assign.tracks.remove.md" `
    "/api/v1/admin/assign/post/admin.assign.tracks.remove.md" `
    "/api/v1/admin/assign/event-assigns/{assignEventId}/tracks/{trackId}/remove) — [`admin/assign/post/admin.assign.tracks.remove.md`](../../../admin/assign/post/admin.assign.tracks.remove.md"
Fix-File "staff\assign\post\staff.assign.tracks.restore.md" `
    "/api/v1/admin/assign/post/admin.assign.tracks.restore.md" `
    "/api/v1/admin/assign/event-assigns/{assignEventId}/tracks/{trackId}/restore) — [`admin/assign/post/admin.assign.tracks.restore.md`](../../../admin/assign/post/admin.assign.tracks.restore.md"

# ===== STAFF USER =====
Fix-File "staff\user\get\staff.users.md" "/api/v1/admin/user/get/admin.users.md" "/api/v1/admin/users) — [`admin/user/get/admin.users.md`](../../../admin/user/get/admin.users.md"
Fix-File "staff\user\get\staff.users.detail.md" "/api/v1/admin/user/get/admin.users.detail.md" "/api/v1/admin/users/{userId}) — [`admin/user/get/admin.users.detail.md`](../../../admin/user/get/admin.users.detail.md"
Fix-File "staff\user\get\staff.users.teams.md" "/api/v1/admin/user/get/admin.users.teams.md" "/api/v1/admin/users/{userId}/teams) — [`admin/user/get/admin.users.teams.md`](../../../admin/user/get/admin.users.teams.md"

# ===== STAFF NOTIFICATION =====
Fix-File "staff\notification\get\staff.notifications.md" "/api/v1/admin/notification/get/admin.notifications.md" "/api/v1/admin/notifications) — [`admin/notification/get/admin.notifications.md`](../../../admin/notification/get/admin.notifications.md"
Fix-File "staff\notification\get\staff.notifications.detail.md" "/api/v1/admin/notification/get/admin.notifications.detail.md" "/api/v1/admin/notifications/{notificationId}) — [`admin/notification/get/admin.notifications.detail.md`](../../../admin/notification/get/admin.notifications.detail.md"
Fix-File "staff\notification\get\staff.notifications.recent.md" "/api/v1/admin/notification/get/admin.notifications.recent.md" "/api/v1/admin/notifications/recent) — [`admin/notification/get/admin.notifications.recent.md`](../../../admin/notification/get/admin.notifications.recent.md"
Fix-File "staff\notification\post\staff.notifications.create.md" "/api/v1/admin/notification/post/admin.notifications.md" "/api/v1/admin/notifications) — [`admin/notification/post/admin.notifications.md`](../../../admin/notification/post/admin.notifications.md"
Fix-File "staff\notification\patch\staff.notifications.update.md" "/api/v1/admin/notification/patch/admin.notifications.update.md" "/api/v1/admin/notifications/{notificationId}) — [`admin/notification/patch/admin.notifications.update.md`](../../../admin/notification/patch/admin.notifications.update.md"
Fix-File "staff\notification\post\staff.notifications.delete.md" "/api/v1/admin/notification/post/admin.notifications.delete.md" "/api/v1/admin/notifications/{notificationId}/delete) — [`admin/notification/post/admin.notifications.delete.md`](../../../admin/notification/post/admin.notifications.delete.md"
Fix-File "staff\notification\post\staff.notifications.restore.md" "/api/v1/admin/notification/post/admin.notifications.restore.md" "/api/v1/admin/notifications/{notificationId}/restore) — [`admin/notification/post/admin.notifications.restore.md`](../../../admin/notification/post/admin.notifications.restore.md"

# ===== STAFF REPORT =====
Fix-File "staff\report\get\staff.reports.md" "/api/v1/admin/report/get/admin.reports.list.md" "/api/v1/admin/reports) — [`admin/report/get/admin.reports.list.md`](../../../admin/report/get/admin.reports.list.md"
Fix-File "staff\report\get\staff.reports.detail.md" "/api/v1/admin/report/get/admin.reports.detail.md" "/api/v1/admin/reports/{reportId}) — [`admin/report/get/admin.reports.detail.md`](../../../admin/report/get/admin.reports.detail.md"
Fix-File "staff\report\get\staff.reports.recent.md" "/api/v1/admin/report/get/admin.reports.recent.md" "/api/v1/admin/reports/recent) — [`admin/report/get/admin.reports.recent.md`](../../../admin/report/get/admin.reports.recent.md"
Fix-File "staff\report\patch\staff.reports.status.md" "/api/v1/admin/report/patch/admin.reports.status.md" "/api/v1/admin/reports/{reportId}/status) — [`admin/report/patch/admin.reports.status.md`](../../../admin/report/patch/admin.reports.status.md"

# ===== STAFF REGISTER-TEAM =====
Fix-File "staff\register-team\get\staff.events.register-teams.md" "/api/v1/admin/register-team/get/admin.events.register-teams.md" "/api/v1/admin/events/{eventId}/register-teams) — [`admin/register-team/get/admin.events.register-teams.md`](../../../admin/register-team/get/admin.events.register-teams.md"
Fix-File "staff\register-team\get\staff.register-teams.detail.md" "/api/v1/admin/register-team/get/admin.register-teams.detail.md" "/api/v1/admin/register-teams/{registerTeamId}) — [`admin/register-team/get/admin.register-teams.detail.md`](../../../admin/register-team/get/admin.register-teams.detail.md"
Fix-File "staff\register-team\get\staff.teams.register-teams.md" "/api/v1/admin/register-team/get/admin.teams.register-teams.md" "/api/v1/admin/teams/{teamId}/register-teams) — [`admin/register-team/get/admin.teams.register-teams.md`](../../../admin/register-team/get/admin.teams.register-teams.md"
Fix-File "staff\register-team\get\staff.users.events.md" "/api/v1/admin/register-team/get/admin.users.events.md" "/api/v1/admin/users/{userId}/events) — [`admin/register-team/get/admin.users.events.md`](../../../admin/register-team/get/admin.users.events.md"
Fix-File "staff\register-team\patch\staff.register-teams.update.md" "/api/v1/admin/register-team/patch/admin.register-teams.update.md" "/api/v1/admin/register-teams/{registerTeamId}) — [`admin/register-team/patch/admin.register-teams.update.md`](../../../admin/register-team/patch/admin.register-teams.update.md"
Fix-File "staff\register-team\post\staff.register-teams.approve.md" "/api/v1/admin/register-team/post/admin.register-teams.approve.md" "/api/v1/admin/register-teams/{registerTeamId}/approve) — [`admin/register-team/post/admin.register-teams.approve.md`](../../../admin/register-team/post/admin.register-teams.approve.md"
Fix-File "staff\register-team\post\staff.register-teams.reject.md" "/api/v1/admin/register-team/post/admin.register-teams.reject.md" "/api/v1/admin/register-teams/{registerTeamId}/reject) — [`admin/register-team/post/admin.register-teams.reject.md`](../../../admin/register-team/post/admin.register-teams.reject.md"
Fix-File "staff\register-team\post\staff.register-teams.ban.md" "/api/v1/admin/register-team/post/admin.register-teams.ban.md" "/api/v1/admin/register-teams/{registerTeamId}/ban) — [`admin/register-team/post/admin.register-teams.ban.md`](../../../admin/register-team/post/admin.register-teams.ban.md"
Fix-File "staff\register-team\post\staff.register-teams.unban.md" "/api/v1/admin/register-team/post/admin.register-teams.unban.md" "/api/v1/admin/register-teams/{registerTeamId}/unban) — [`admin/register-team/post/admin.register-teams.unban.md`](../../../admin/register-team/post/admin.register-teams.unban.md"
Fix-File "staff\register-team\post\staff.register-teams.assign-next-round.md" "/api/v1/admin/register-team/post/admin.register-teams.assign-next-round.md" "/api/v1/admin/register-teams/{registerTeamId}/assign-next-round) — [`admin/register-team/post/admin.register-teams.assign-next-round.md`](../../../admin/register-team/post/admin.register-teams.assign-next-round.md"
Fix-File "staff\register-team\post\staff.register-teams.revert-previous-round.md" "/api/v1/admin/register-team/post/admin.register-teams.revert-previous-round.md" "/api/v1/admin/register-teams/{registerTeamId}/revert-previous-round) — [`admin/register-team/post/admin.register-teams.revert-previous-round.md`](../../../admin/register-team/post/admin.register-teams.revert-previous-round.md"
Fix-File "staff\register-team\post\staff.register-teams.assign-track-topic.md" "/api/v1/admin/register-team/post/admin.register-teams.assign-track-topic.md" "/api/v1/admin/register-teams/{registerTeamId}/assign-track-topic) — [`admin/register-team/post/admin.register-teams.assign-track-topic.md`](../../../admin/register-team/post/admin.register-teams.assign-track-topic.md"
Fix-File "staff\register-team\post\staff.register-teams.remove-track-topic.md" "/api/v1/admin/register-team/post/admin.register-teams.remove-track-topic.md" "/api/v1/admin/register-teams/{registerTeamId}/remove-track-topic) — [`admin/register-team/post/admin.register-teams.remove-track-topic.md`](../../../admin/register-team/post/admin.register-teams.remove-track-topic.md"

# ===== STAFF TEAM =====
Fix-File "staff\team\get\staff.teams.md" "/api/v1/admin/team/get/admin.teams.md" "/api/v1/admin/teams) — [`admin/team/get/admin.teams.md`](../../../admin/team/get/admin.teams.md"
Fix-File "staff\team\get\staff.teams.detail.md" "/api/v1/admin/team/get/admin.teams.detail.md" "/api/v1/admin/teams/{teamId}) — [`admin/team/get/admin.teams.detail.md`](../../../admin/team/get/admin.teams.detail.md"
Fix-File "staff\team\get\staff.teams.events.md" "/api/v1/admin/team/get/admin.teams.events.md" "/api/v1/admin/teams/{teamId}/events) — [`admin/team/get/admin.teams.events.md`](../../../admin/team/get/admin.teams.events.md"

# ===== STAFF SUBMISSION =====
Fix-File "staff\submission\get\staff.submissions.md" "/api/v1/admin/submission/get/admin.events.submissions.md" "/api/v1/admin/events/{eventId}/submissions) — [`admin/submission/get/admin.events.submissions.md`](../../../admin/submission/get/admin.events.submissions.md"
Fix-File "staff\submission\get\staff.submissions.detail.md" "/api/v1/admin/submission/get/admin.submissions.detail.md" "/api/v1/admin/submissions/{submissionId}) — [`admin/submission/get/admin.submissions.detail.md`](../../../admin/submission/get/admin.submissions.detail.md"
Fix-File "staff\submission\get\staff.submissions.by-track.md" "/api/v1/admin/submission/get/admin.tracks.submissions.md" "/api/v1/admin/tracks/{trackId}/submissions) — [`admin/submission/get/admin.tracks.submissions.md`](../../../admin/submission/get/admin.tracks.submissions.md"
Fix-File "staff\submission\get\staff.submissions.by-round.md" "/api/v1/admin/submission/get/admin.rounds.submissions.md" "/api/v1/admin/rounds/{roundId}/submissions) — [`admin/submission/get/admin.rounds.submissions.md`](../../../admin/submission/get/admin.rounds.submissions.md"
Fix-File "staff\submission\get\staff.submissions.by-register-team.md" "/api/v1/admin/submission/get/admin.register-teams.submissions.md" "/api/v1/admin/register-teams/{registerTeamId}/submissions) — [`admin/submission/get/admin.register-teams.submissions.md`](../../../admin/submission/get/admin.register-teams.submissions.md"

# ===== STAFF SCORE =====
Fix-File "staff\score\get\staff.scores.detail.md" "/api/v1/admin/score/get/admin.scores.detail.md" "/api/v1/admin/scores/{scoreId}) — [`admin/score/get/admin.scores.detail.md`](../../../admin/score/get/admin.scores.detail.md"
Fix-File "staff\score\get\staff.scores.items.md" "/api/v1/admin/score/get/admin.scores.items.md" "/api/v1/admin/scores/{scoreId}/items) — [`admin/score/get/admin.scores.items.md`](../../../admin/score/get/admin.scores.items.md"
Fix-File "staff\score\get\staff.scores.grader-scores.md" "/api/v1/admin/score/get/admin.submissions.grader-scores.md" "/api/v1/admin/submissions/{submissionId}/grader-scores) — [`admin/score/get/admin.submissions.grader-scores.md`](../../../admin/score/get/admin.submissions.grader-scores.md"
Fix-File "staff\score\get\staff.scores.team-round.md" "/api/v1/admin/score/get/admin.scores.team-round-score.md" "/api/v1/admin/rounds/{roundId}/register-teams/{registerTeamId}/scores) — [`admin/score/get/admin.scores.team-round-score.md`](../../../admin/score/get/admin.scores.team-round-score.md"
Fix-File "staff\score\get\staff.score-items.detail.md" "/api/v1/admin/score/get/admin.score-items.detail.md" "/api/v1/admin/score-items/{scoreItemId}) — [`admin/score/get/admin.score-items.detail.md`](../../../admin/score/get/admin.score-items.detail.md"

# ===== STAFF LEADERBOARD =====
Fix-File "staff\leaderboard\get\staff.events.leaderboard.md" "/api/v1/admin/leaderboard/get/admin.leaderboard.event.md" "/api/v1/admin/events/{eventId}/leaderboard) — [`admin/leaderboard/get/admin.leaderboard.event.md`](../../../admin/leaderboard/get/admin.leaderboard.event.md"
Fix-File "staff\leaderboard\get\staff.events.chapter-leaderboard.md" "/api/v1/admin/leaderboard/get/admin.leaderboard.chapter.md" "/api/v1/admin/events/chapter/{year}/leaderboard) — [`admin/leaderboard/get/admin.leaderboard.chapter.md`](../../../admin/leaderboard/get/admin.leaderboard.chapter.md"
Fix-File "staff\leaderboard\get\staff.rounds.leaderboard.md" "/api/v1/admin/leaderboard/get/admin.leaderboard.round.md" "/api/v1/admin/rounds/{roundId}/leaderboard) — [`admin/leaderboard/get/admin.leaderboard.round.md`](../../../admin/leaderboard/get/admin.leaderboard.round.md"
Fix-File "staff\leaderboard\post\staff.chapter.hide.md" "/api/v1/admin/leaderboard/post/admin.chapter.hide.md" "/api/v1/admin/events/chapter/{year}/leaderboard/hide) — [`admin/leaderboard/post/admin.chapter.hide.md`](../../../admin/leaderboard/post/admin.chapter.hide.md"
Fix-File "staff\leaderboard\post\staff.chapter.publish.md" "/api/v1/admin/leaderboard/post/admin.chapter.publish.md" "/api/v1/admin/events/chapter/{year}/leaderboard/publish) — [`admin/leaderboard/post/admin.chapter.publish.md`](../../../admin/leaderboard/post/admin.chapter.publish.md"
Fix-File "staff\leaderboard\post\staff.leaderboard.events.hide.md" "/api/v1/admin/leaderboard/post/admin.leaderboard.events.hide.md" "/api/v1/admin/events/{eventId}/leaderboard/hide) — [`admin/leaderboard/post/admin.leaderboard.events.hide.md`](../../../admin/leaderboard/post/admin.leaderboard.events.hide.md"
Fix-File "staff\leaderboard\post\staff.leaderboard.events.publish.md" "/api/v1/admin/leaderboard/post/admin.leaderboard.events.publish.md" "/api/v1/admin/events/{eventId}/leaderboard/publish) — [`admin/leaderboard/post/admin.leaderboard.events.publish.md`](../../../admin/leaderboard/post/admin.leaderboard.events.publish.md"

Write-Host "`n=== Done! Fixed $fixed files ===" -F Green
