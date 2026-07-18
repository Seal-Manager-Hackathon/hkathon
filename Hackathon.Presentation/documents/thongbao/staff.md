# Staff — Notification Mapping
> Controller: StaffRegisterTeamController / StaffReportController / StaffAssignController

## Register Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| PUT | register-teams/{registerId}/approve | ApproveRegisterTeam | Your team {TênTeam} has been approved for {TênEvent} | Personal → all members |
| PUT | register-teams/{registerId}/reject | RejectRegisterTeam | Your team {TênTeam} has been rejected: {lý do} | Personal → team leader |
| PATCH | register-teams/{registerId}/ban | BanRegisterTeam | Your team {TênTeam} has been banned: {lý do} | Personal → team leader |
| PATCH | register-teams/{registerId}/unban | UnbanRegisterTeam | Your team {TênTeam} has been unbanned | Personal → team leader |
| POST | events/{eventId}/register-teams/{registerTeamId}/track | AssignTrackTopic | Your team {TênTeam} has been assigned track for {TênEvent} | Personal → all members |
| POST | events/{eventId}/register-teams/{registerTeamId}/topic | AssignTrackTopic | Your team {TênTeam} has been assigned topic for {TênEvent} | Personal → all members |
| POST | events/{eventId}/register-teams/{registerTeamId}/assign-next-round | AssignToNextRound | Your team {TênTeam} has advanced to round {RoundNo} | Personal → all members |
| POST | events/{eventId}/register-teams/{registerTeamId}/revert-previous-round | RevertToPreviousRound | Your team {TênTeam} has been moved back to round {RoundNo} | Personal → all members |

## Report
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| PUT | reports/{reportId}/status | UpdateReportStatus | Your report has been {status} | Personal → reporter |
| POST | reports/{reportId}/approve-regrade | ApproveRegrade | Your regrade request has been approved | Personal → team leader |

## Assign Lecturer
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | events/{eventId}/assign-lecturers | AssignLecturerToEvent | You have been assigned as {role} for {TênEvent} | Personal → lecturer |
| DELETE | events/{eventId}/remove-lecturer/{id} | RemoveAssignEvent | You have been removed from event {TênEvent} | Personal → lecturer |
| POST | events/{eventId}/tracks/{trackId}/assign-lecturers | AssignTrackToEvent | You have been assigned to track {TênTrack} in {TênEvent} | Personal → lecturer |
| DELETE | events/{eventId}/tracks/{trackId}/remove-lecturer/{id} | RemoveTrackFromEvent | You have been removed from track {TênTrack} in {TênEvent} | Personal → lecturer |
