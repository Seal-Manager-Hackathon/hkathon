namespace Hackathon.Application.Services.Admin.Invitation;

public interface IInvitationService
{
    Task<GetInvitationsResponse> GetInvitations(Guid teamId, string? status, string? keyword, int pageIndex, int pageSize);
}
