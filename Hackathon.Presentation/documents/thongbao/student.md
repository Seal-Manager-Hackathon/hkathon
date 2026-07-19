# Student — Notification Mapping
> Controller: StudentInvitationController / StudentTeamController / StudentRegisterTeamController / StudentSubmissionController

## Invitation
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
| POST | teams/{teamId}/invitations | SendInvitation | {NgườiMời} invited you to team {TênTeam} | Personal → user được mời |
<<<<<<< HEAD
| POST | invitations/{id}/accept | AcceptInvitation | {NgườiMới} has joined team {TênTeam} | Team → teamId |
=======
| POST | invitations/{id}/accept | AcceptInvitation | {NgườiMới} has joined team {TênTeam} | Personal → team leader |
>>>>>>> 61a7af590f5aac5012240b2771778e36a3a6a8eb
| POST | invitations/{id}/reject | RejectInvitation | {NgườiTừChối} has rejected your invitation | Personal → team leader |

## Team
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
<<<<<<< HEAD
| POST | teams/{teamId}/disband | DisbandTeam | Team {TênTeam} has been disbanded | Team → teamId |
| POST | teams/{teamId}/kick | KickMember | You have been removed from team {TênTeam} | Personal → member bị kick |
| POST | teams/{teamId}/transfer-leader | ChangeLeader | You are now the leader of team {TênTeam} | Personal → new leader |
| POST | teams/{teamId}/leave | LeaveTeam | {Người} has left team {TênTeam} | Team → teamId |
=======
| POST | teams/{teamId}/disband | DisbandTeam | Team {TênTeam} has been disbanded | Personal → all members |
| POST | teams/{teamId}/kick | KickMember | You have been removed from team {TênTeam} | Personal → member bị kick |
| POST | teams/{teamId}/transfer-leader | ChangeLeader | You are now the leader of team {TênTeam} | Personal → new leader |
| POST | teams/{teamId}/leave | LeaveTeam | {Người} has left team {TênTeam} | Personal → leader |
>>>>>>> 61a7af590f5aac5012240b2771778e36a3a6a8eb

## Register Event
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
<<<<<<< HEAD
| POST | register-teams | CreateRegisterTeam | Your team {TênTeam} has registered for {TênEvent} | Team → teamId + Personal → mỗi Admin/Staff |
=======
| POST | register-teams | CreateRegisterTeam | Your team {TênTeam} has registered for {TênEvent} | Personal → team leader |
>>>>>>> 61a7af590f5aac5012240b2771778e36a3a6a8eb

## Submission
| Method | API | Service | Nội dung | Target |
|--------|-----|---------|----------|--------|
<<<<<<< HEAD
| POST | register-teams/submissions | CreateSubmission | Your team {TênTeam} has submitted for round {RoundNo} | Team → teamId |
=======
| POST | register-teams/submissions | CreateSubmission | Your team {TênTeam} has submitted for round {RoundNo} | Personal → team leader |
>>>>>>> 61a7af590f5aac5012240b2771778e36a3a6a8eb
