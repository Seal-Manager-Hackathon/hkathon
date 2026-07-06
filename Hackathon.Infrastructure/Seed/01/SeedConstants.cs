namespace Hackathon.Infrastructure.Seed;

public static class SeedConstants
{
    public static readonly DateTimeOffset CreatedAt = new(2026, 6, 11, 0, 0, 0, TimeSpan.Zero);

    public static readonly Guid AdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid StaffRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid StudentRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid LecturerRoleId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static readonly Guid MentorEventRoleId = Guid.Parse("55555555-5555-5555-5555-555555555555");
    public static readonly Guid JudgeEventRoleId = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static readonly Guid StaffEventRoleId = Guid.Parse("77777777-7777-7777-7777-777777777777");

    public static readonly Guid AdminUserId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid StaffUserId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid MentorUserId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid JudgeUserId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    public static readonly Guid StudentLeaderUserId = Guid.Parse("10000000-0000-0000-0000-000000000005");
    public static readonly Guid StudentMemberUserId = Guid.Parse("10000000-0000-0000-0000-000000000006");
    public static readonly Guid GreenLeaderUserId = Guid.Parse("10000000-0000-0000-0000-000000000007");

    public static readonly Guid SealHackathonEventId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid IdeaRoundId = Guid.Parse("21000000-0000-0000-0000-000000000001");
    public static readonly Guid FinalRoundId = Guid.Parse("21000000-0000-0000-0000-000000000002");
    public static readonly Guid IdeaCriteriaTemplateId = Guid.Parse("22000000-0000-0000-0000-000000000001");
    public static readonly Guid FinalCriteriaTemplateId = Guid.Parse("22000000-0000-0000-0000-000000000002");
    public static readonly Guid InnovationCriteriaItemId = Guid.Parse("23000000-0000-0000-0000-000000000001");
    public static readonly Guid FeasibilityCriteriaItemId = Guid.Parse("23000000-0000-0000-0000-000000000002");
    public static readonly Guid TechnicalCriteriaItemId = Guid.Parse("23000000-0000-0000-0000-000000000003");
    public static readonly Guid PresentationCriteriaItemId = Guid.Parse("23000000-0000-0000-0000-000000000004");

    public static readonly Guid AiTrackId = Guid.Parse("24000000-0000-0000-0000-000000000001");
    public static readonly Guid GreenTrackId = Guid.Parse("24000000-0000-0000-0000-000000000002");
    public static readonly Guid AiTopicId = Guid.Parse("25000000-0000-0000-0000-000000000001");
    public static readonly Guid GreenTopicId = Guid.Parse("25000000-0000-0000-0000-000000000002");

    public static readonly Guid ChampionAwardId = Guid.Parse("26000000-0000-0000-0000-000000000001");
    public static readonly Guid RunnerUpAwardId = Guid.Parse("26000000-0000-0000-0000-000000000002");

    public static readonly Guid SeedInnovatorsTeamId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid GreenCodersTeamId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid SeedInnovatorsRegisterTeamId = Guid.Parse("31000000-0000-0000-0000-000000000001");
    public static readonly Guid GreenCodersRegisterTeamId = Guid.Parse("31000000-0000-0000-0000-000000000002");

    public static readonly Guid SeedInnovatorsIdeaRoundDetailId = Guid.Parse("32000000-0000-0000-0000-000000000001");
    public static readonly Guid SeedInnovatorsFinalRoundDetailId = Guid.Parse("32000000-0000-0000-0000-000000000002");
    public static readonly Guid GreenCodersIdeaRoundDetailId = Guid.Parse("32000000-0000-0000-0000-000000000003");
    public static readonly Guid GreenCodersFinalRoundDetailId = Guid.Parse("32000000-0000-0000-0000-000000000004");

    public static readonly Guid SeedInnovatorsIdeaSubmissionId = Guid.Parse("33000000-0000-0000-0000-000000000001");
    public static readonly Guid SeedInnovatorsFinalSubmissionId = Guid.Parse("33000000-0000-0000-0000-000000000002");
    public static readonly Guid GreenCodersIdeaSubmissionId = Guid.Parse("33000000-0000-0000-0000-000000000003");
    public static readonly Guid GreenCodersFinalSubmissionId = Guid.Parse("33000000-0000-0000-0000-000000000004");

    public static readonly Guid MentorAssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid JudgeAssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000002");
    public static readonly Guid StaffAssignEventId = Guid.Parse("40000000-0000-0000-0000-000000000003");
    public static readonly Guid MentorAiAssignTrackId = Guid.Parse("41000000-0000-0000-0000-000000000001");
    public static readonly Guid JudgeAiAssignTrackId = Guid.Parse("41000000-0000-0000-0000-000000000002");
    public static readonly Guid JudgeGreenAssignTrackId = Guid.Parse("41000000-0000-0000-0000-000000000003");

    public static readonly Guid SeedInnovatorsIdeaScoreId = Guid.Parse("50000000-0000-0000-0000-000000000001");
    public static readonly Guid SeedInnovatorsFinalScoreId = Guid.Parse("50000000-0000-0000-0000-000000000002");
    public static readonly Guid GreenCodersIdeaScoreId = Guid.Parse("50000000-0000-0000-0000-000000000003");
    public static readonly Guid GreenCodersFinalScoreId = Guid.Parse("50000000-0000-0000-0000-000000000004");

    public static readonly Guid LeaderBoardId = Guid.Parse("60000000-0000-0000-0000-000000000001");
}
