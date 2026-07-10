using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users { get; set; }

    public DbSet<RefreshTokens> RefreshTokens { get; set; }
    public DbSet<ResetPasswords> ResetPasswords { get; set; }
    public DbSet<EmailVerifications> EmailVerifications { get; set; }
    public DbSet<Events> Events { get; set; }
    public DbSet<Rounds> Rounds { get; set; }
    public DbSet<CriteriaTemplates> CriteriaTemplates { get; set; }
    public DbSet<CriteriaItems> CriteriaItems { get; set; }
    public DbSet<Tracks> Tracks { get; set; }
    public DbSet<Topics> Topics { get; set; }
    public DbSet<Awards> Awards { get; set; }
    public DbSet<LeaderBoards> LeaderBoards { get; set; }
    public DbSet<LeaderBoardDetails> LeaderBoardDetails { get; set; }
    public DbSet<Teams> Teams { get; set; }
    public DbSet<TeamDetails> TeamDetails { get; set; }
    public DbSet<RegisterTeams> RegisterTeams { get; set; }
    public DbSet<RoundDetails> RoundDetails { get; set; }
    public DbSet<Submissions> Submissions { get; set; }
    public DbSet<Invitations> Invitations { get; set; }
    public DbSet<Notifications> Notifications { get; set; }
    public DbSet<EventRoles> EventRoles { get; set; }
    public DbSet<AssignEvents> AssignEvents { get; set; }
    public DbSet<AssignTracks> AssignTracks { get; set; }
    public DbSet<Scores> Scores { get; set; }
    public DbSet<ScoreItems> ScoreItems { get; set; }
    public DbSet<MentorNotifications> MentorNotifications { get; set; }
    public DbSet<Reports> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmailVerifications>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Events>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Invitations>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Notifications>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Notifications>().Property(x => x.TargetType).HasConversion<string>();
        modelBuilder.Entity<RegisterTeams>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Reports>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Submissions>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<TeamDetails>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<Users>().Property(x => x.Status).HasConversion<string>();

        modelBuilder.Entity<EventRoles>().Property(x => x.Name).HasConversion<string>();

        modelBuilder.Entity<RefreshTokens>()
            .HasOne(refreshToken => refreshToken.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.UserId);

        modelBuilder.Entity<ResetPasswords>()
            .HasOne(resetPassword => resetPassword.User)
            .WithMany(user => user.ResetPasswords)
            .HasForeignKey(resetPassword => resetPassword.UserId);

        modelBuilder.Entity<EmailVerifications>()
            .HasOne(emailVerification => emailVerification.User)
            .WithMany(user => user.EmailVerifications)
            .HasForeignKey(emailVerification => emailVerification.UserId);

        modelBuilder.Entity<Rounds>()
            .HasOne(round => round.Event)
            .WithMany(@event => @event.Rounds)
            .HasForeignKey(round => round.EventId);

        modelBuilder.Entity<Tracks>()
            .HasOne(track => track.Event)
            .WithMany(@event => @event.Tracks)
            .HasForeignKey(track => track.EventId);

        modelBuilder.Entity<Awards>()
            .HasOne(award => award.Event)
            .WithMany(@event => @event.Awards)
            .HasForeignKey(award => award.EventId);

        modelBuilder.Entity<LeaderBoards>()
            .HasOne(leaderBoard => leaderBoard.Event)
            .WithOne(@event => @event.LeaderBoard)
            .HasForeignKey<LeaderBoards>(leaderBoard => leaderBoard.EventId);

        modelBuilder.Entity<CriteriaTemplates>()
            .HasOne(criteriaTemplate => criteriaTemplate.Round)
            .WithMany(round => round.CriteriaTemplates)
            .HasForeignKey(criteriaTemplate => criteriaTemplate.RoundId);

        modelBuilder.Entity<CriteriaItems>()
            .HasOne(criteriaItem => criteriaItem.CriteriaTemplate)
            .WithMany(criteriaTemplate => criteriaTemplate.CriteriaItems)
            .HasForeignKey(criteriaItem => criteriaItem.CriteriaTemplateId);

        modelBuilder.Entity<Topics>()
            .HasOne(topic => topic.Track)
            .WithMany(track => track.Topics)
            .HasForeignKey(topic => topic.TrackId);

        modelBuilder.Entity<TeamDetails>()
            .HasOne(teamDetail => teamDetail.Team)
            .WithMany(team => team.TeamDetails)
            .HasForeignKey(teamDetail => teamDetail.TeamId);

        modelBuilder.Entity<TeamDetails>()
            .HasOne(teamDetail => teamDetail.User)
            .WithMany(user => user.TeamDetails)
            .HasForeignKey(teamDetail => teamDetail.UserId);

        modelBuilder.Entity<RegisterTeams>()
            .HasOne(registerTeam => registerTeam.Team)
            .WithMany(team => team.RegisterTeams)
            .HasForeignKey(registerTeam => registerTeam.TeamId);

        modelBuilder.Entity<RegisterTeams>()
            .HasOne(registerTeam => registerTeam.Event)
            .WithMany(eventEntity => eventEntity.RegisterTeams)
            .HasForeignKey(registerTeam => registerTeam.EventId);

        modelBuilder.Entity<RegisterTeams>()
            .HasOne(registerTeam => registerTeam.Track)
            .WithMany()
            .HasForeignKey(registerTeam => registerTeam.TrackId);

        modelBuilder.Entity<RegisterTeams>()
            .HasOne(registerTeam => registerTeam.Topic)
            .WithMany()
            .HasForeignKey(registerTeam => registerTeam.TopicId);

        modelBuilder.Entity<RoundDetails>()
            .HasOne(roundDetail => roundDetail.Round)
            .WithMany(round => round.RoundDetails)
            .HasForeignKey(roundDetail => roundDetail.RoundId);

        modelBuilder.Entity<RoundDetails>()
            .HasOne(roundDetail => roundDetail.RegisterTeam)
            .WithMany(registerTeam => registerTeam.RoundDetails)
            .HasForeignKey(roundDetail => roundDetail.RegisterTeamId);

        modelBuilder.Entity<Submissions>()
            .HasOne(submission => submission.RoundDetail)
            .WithMany(roundDetail => roundDetail.Submissions)
            .HasForeignKey(submission => submission.RoundDetailId);

        modelBuilder.Entity<Invitations>()
            .HasOne(invitation => invitation.Team)
            .WithMany(team => team.Invitations)
            .HasForeignKey(invitation => invitation.TeamId);

        modelBuilder.Entity<Invitations>()
            .HasOne(invitation => invitation.User)
            .WithMany(user => user.Invitations)
            .HasForeignKey(invitation => invitation.UserId);

        modelBuilder.Entity<Notifications>()
            .HasOne(notification => notification.Team)
            .WithMany(team => team.Notifications)
            .HasForeignKey(notification => notification.TeamId)
            .IsRequired(false);

        modelBuilder.Entity<Notifications>()
            .HasOne(notification => notification.User)
            .WithMany(user => user.Notifications)
            .HasForeignKey(notification => notification.UserId)
            .IsRequired(false);

        modelBuilder.Entity<AssignEvents>()
            .HasOne(assignEvent => assignEvent.User)
            .WithMany(user => user.AssignEvents)
            .HasForeignKey(assignEvent => assignEvent.UserId);

        modelBuilder.Entity<AssignEvents>()
            .HasOne(assignEvent => assignEvent.EventRole)
            .WithMany(eventRole => eventRole.AssignEvents)
            .HasForeignKey(assignEvent => assignEvent.EventRoleId)
            .IsRequired(false);

        modelBuilder.Entity<AssignEvents>()
            .HasOne(assignEvent => assignEvent.Event)
            .WithMany(@event => @event.AssignEvents)
            .HasForeignKey(assignEvent => assignEvent.EventId);

        modelBuilder.Entity<AssignTracks>()
            .HasOne(assignTrack => assignTrack.AssignEvent)
            .WithMany(assignEvent => assignEvent.AssignTracks)
            .HasForeignKey(assignTrack => assignTrack.AssignEventId);

        modelBuilder.Entity<AssignTracks>()
            .HasOne(assignTrack => assignTrack.Track)
            .WithMany(track => track.AssignTracks)
            .HasForeignKey(assignTrack => assignTrack.TrackId);

        modelBuilder.Entity<MentorNotifications>()
            .HasOne(mentorNotification => mentorNotification.AssignTrack)
            .WithMany(assignTrack => assignTrack.MentorNotifications)
            .HasForeignKey(mentorNotification => mentorNotification.AssignTrackId);

        modelBuilder.Entity<Scores>()
            .HasOne(score => score.Submission)
            .WithMany(submission => submission.Scores)
            .HasForeignKey(score => score.SubmissionId);

        modelBuilder.Entity<Scores>()
            .HasOne(score => score.AssignTrack)
            .WithMany(assignTrack => assignTrack.Scores)
            .HasForeignKey(score => score.AssignTrackId);

        modelBuilder.Entity<Scores>()
            .HasOne(score => score.RetakeFromScore)
            .WithMany(score => score.RetakeScores)
            .HasForeignKey(score => score.RetakeFromScoreId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Scores>()
            .HasIndex(score => score.RetakeFromScoreId)
            .IsUnique();

        modelBuilder.Entity<ScoreItems>()
            .HasOne(scoreItem => scoreItem.ScoreEntity)
            .WithMany(score => score.ScoreItems)
            .HasForeignKey(scoreItem => scoreItem.ScoreId);

        modelBuilder.Entity<ScoreItems>()
            .HasOne(scoreItem => scoreItem.CriteriaItem)
            .WithMany(criteriaItem => criteriaItem.ScoreItems)
            .HasForeignKey(scoreItem => scoreItem.CriteriaItemId);

        modelBuilder.Entity<ScoreItems>()
            .HasOne(scoreItem => scoreItem.AssignTrack)
            .WithMany(assignTrack => assignTrack.ScoreItems)
            .HasForeignKey(scoreItem => scoreItem.AssignTrackId);

        modelBuilder.Entity<Reports>()
            .HasOne(report => report.User)
            .WithMany(user => user.Reports)
            .HasForeignKey(report => report.UserId);

        modelBuilder.Entity<LeaderBoardDetails>()
            .HasOne(leaderBoardDetail => leaderBoardDetail.LeaderBoard)
            .WithMany(leaderBoard => leaderBoard.LeaderBoardDetails)
            .HasForeignKey(leaderBoardDetail => leaderBoardDetail.LeaderBoardId);

        modelBuilder.Entity<LeaderBoardDetails>()
            .HasOne(leaderBoardDetail => leaderBoardDetail.Team)
            .WithMany(team => team.LeaderBoardDetails)
            .HasForeignKey(leaderBoardDetail => leaderBoardDetail.TeamId);
        modelBuilder.SeedEventRoles();
        modelBuilder.SeedUsers();
        modelBuilder.SeedAuthData();
        modelBuilder.SeedEvents();
        modelBuilder.SeedRounds();
        modelBuilder.SeedCriteria();
        modelBuilder.SeedTracks();
        modelBuilder.SeedAwards();
        modelBuilder.SeedTeams();
        modelBuilder.SeedRoundDetails();
        modelBuilder.SeedSubmissions();
        modelBuilder.SeedAssignments();
        modelBuilder.SeedScores();
        modelBuilder.SeedNotifications();
        modelBuilder.SeedReports();
        modelBuilder.SeedLeaderBoards();
        modelBuilder.SeedDemoData();
        modelBuilder.SeedFPTData();
    }
}
