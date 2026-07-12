namespace Hackathon.Application.Services.Student.Invitation;

public interface IInvitationService
{
    Task SendInvitation(Guid teamId, string email);
    Task<GetInvitationsResponse> GetSentInvitations(Guid teamId, int pageIndex, int pageSize);
    Task<GetInvitationsResponse> GetReceivedInvitations(string? keyword, string? status, int pageIndex, int pageSize);
}
