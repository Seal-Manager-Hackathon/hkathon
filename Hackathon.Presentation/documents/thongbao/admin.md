# Admin — Notification Mapping
> Controller: AdminRegisterTeamController / AdminRoundController / AdminAssignController

## Register Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | register-teams/{id}/ban | BanRegisterTeam | Your team {TênTeam} has been banned: {lý do} | Team → teamId |
| POST | register-teams/{id}/unban | UnbanRegisterTeam | Your team {TênTeam} has been unbanned | Team → teamId |
| POST | register-teams/{id}/assign-next-round | AssignToNextRound | Your team {TênTeam} has advanced to round {RoundNo} | Team → teamId |
| POST | register-teams/{id}/revert-previous-round | RevertToPreviousRound | Your team {TênTeam} has been moved back to round {RoundNo} | Team → teamId |

## Round
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | rounds/{id}/end-final | EndRound | Round {RoundNo} of {TênEvent} has ended | System |

## Assign
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| PUT | events/{eventId}/lecturers/{id}/role | ChangeEventRole | Your role in {TênEvent} has been changed to {role} | Personal → lecturer |
