# Business & Authorization Rules

## Authorization
- No dynamic Permission/RBAC tables.
- API authorization is hard-coded by role/action.
- Global roles: `Admin`, `Staff`, `Student`, `Lecturer`.
- Event roles: `Mentor`, `Judge`.
- Event/track APIs must check global role + `AssignEvents` + `AssignTracks` + business rule.

## Account/Profile
- Student must complete profile before creating/joining a team.
- Profile fields live in `Users`; no `Profile` table.
- Banned/disabled user cannot log in.

## Team/Register
- Team creator is leader.
- Team leader submits event registration.
- Staff approves/rejects whole teams, not individual members.
- A student cannot join multiple teams in the same event.
- Member count must satisfy `Events.MinMember` and `Events.MaxMember`.
- Approved team registration locks members for that event flow.

## Event/Round/Criteria
- Admin creates event and event setup.
- Event has many rounds.
- Round has many criteria templates.
- Criteria template has many criteria items.
- Criteria should not be changed freely after scoring starts.

## Track/Topic/Draw
- Track belongs to event.
- Topic belongs to track.
- `Topics` are exam topics/papers; no `ExamPapers` table.
- Track/topic draw is offline; staff records result.
- Track is inferred by `RegisterTeams.TopicId -> Topics.TrackId`.
- `RoundDetails` connects `Rounds` and `RegisterTeams`.

## Mentor/Judge
- Lecturer becomes mentor/judge only through assignment.
- Mentor supports and sends notices; mentor does not score.
- Judge scores assigned track submissions by criteria.
- A lecture should not be mentor and judge in the same event.

## Submission/Score
- Team leader submits official submissions.
- A team can submit multiple times; use latest valid submission before deadline.
- One submission can be scored by multiple judges.
- Round score = average of judges' `Scores.TotalScore`.
- Score edits update existing record before finalized; no score version table.

## Report/Regrade
- Reports/regrade use `Reports`; no separate `Appeals` table.
- Team leader sends official regrade request.
- One regrade request per round.
- Regrade result is final.

## Leaderboard
- Event leaderboard = total round scores.
- Year leaderboard = total event leaderboard scores.
- `LeaderBoardDetails.LevelAward` stores award level/result.

## Audit
- No `AuditLogs` table in current DBML.
- Important actions should be logged at service/app level if needed.
