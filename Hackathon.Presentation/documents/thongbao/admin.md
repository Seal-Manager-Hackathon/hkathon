# Admin — Notification Mapping
> Controller: AdminEventController / AdminRegisterTeamController / AdminRoundController / AdminAwardController / AdminAssignController

## Event
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | hackathons/{eventId}/close | DeleteEvent | Event {TênEvent} has been closed | Personal → all participants |
| POST | hackathons/{eventId}/publish-leaderboard | PublishLeaderboard | Leaderboard for {TênEvent} has been published | Personal → all participants |
| POST | hackathons/{eventId}/assign-award | AssignAward | Your team won {tên giải} in {TênEvent} | Personal → team |

## Register Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | register-teams/{id}/ban | BanRegisterTeam | Your team {TênTeam} has been banned: {lý do} | Personal → team leader |
| POST | register-teams/{id}/unban | UnbanRegisterTeam | Your team {TênTeam} has been unbanned | Personal → team leader |
| POST | register-teams/{id}/assign-next-round | AssignToNextRound | Your team {TênTeam} has advanced to round {RoundNo} | Personal → all members |
| POST | register-teams/{id}/revert-previous-round | RevertToPreviousRound | Your team {TênTeam} has been moved back to round {RoundNo} | Personal → all members |

## Round
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | rounds/{id}/end-final | EndRound | Round {RoundNo} of {TênEvent} has ended | Personal → all teams |

## Assign
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| PUT | events/{eventId}/lecturers/{id}/role | ChangeEventRole | Your role in {TênEvent} has been changed to {role} | Personal → lecturer |
