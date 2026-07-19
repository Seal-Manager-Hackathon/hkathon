namespace Hackathon.Application.Common;

public static class NotificationMessage
{
    public static class Invitation
    {
        public const string InvitationReceived = "{0} invited you to team {1}";
        public const string MemberJoined = "{0} {1} has joined team {2}";
        public const string InvitationDeclined = "{0} {1} has rejected your invitation to team {2}";
    }

    public static class Team
    {
        public const string TeamDisbanded = "Team {0} has been disbanded";
        public const string MemberRemoved = "You have been removed from team {0}";
        public const string NewLeader = "You are now the leader of team {0}";
        public const string MemberLeft = "{0} {1} has left team {2}";
    }

    public static class RegisterEvent
    {
        public const string Registered = "Your team {0} has registered for {1}";
        public const string Approved = "Your team {0} has been approved for {1}";
        public const string Rejected = "Your team {0} has been rejected: {1}";
        public const string Banned = "Your team {0} has been banned: {1}";
        public const string Unbanned = "Your team {0} has been unbanned";
        public const string TrackAssigned = "Your team {0} has been assigned track for {1}";
        public const string TopicAssigned = "Your team {0} has been assigned topic for {1}";
        public const string AdvancedToNextRound = "Your team {0} has advanced to round {1}";
        public const string MovedBackToPreviousRound = "Your team {0} has been moved back to round {1}";
        public const string TrackTopicRemoved = "Your team {0} has been removed from track {1} and topic {2} in {3}";
    }

    public static class Submission
    {
        public const string Submitted = "Your team {0} has submitted for round {1}";
        public const string Scored = "Your submission for round {0} has been scored";
    }

    public static class Report
    {
        public const string StatusUpdated = "Your report \"{0}\" has been {1}";
        public const string RegradeApproved = "Your regrade request has been approved";
    }

    public static class Event
    {
        public const string Closed = "Event {0} has been closed";
        public const string LeaderboardPublished = "Leaderboard for {0} has been published";
        public const string AwardWon = "Your team won {0} in {1}";
        public const string RoundEnded = "Round {0} of {1} has ended";
    }

    public static class Assignment
    {
        public const string AssignedAsRole = "You have been assigned as {0} for {1}";
        public const string RemovedFromEvent = "You have been removed from event {0}";
        public const string RestoredToEvent = "You have been restored to event {0}";
        public const string AssignedToTrack = "You have been assigned to track {0} in {1}";
        public const string RemovedFromTrack = "You have been removed from track {0} in {1}";
        public const string RestoredToTrack = "You have been restored to track {0} in {1}";
        public const string RoleChanged = "Your role in {0} has been changed from {1} to {2}";
        public const string ReEnabled = "Your assignment to event {0} has been reactivated";
    }
}
