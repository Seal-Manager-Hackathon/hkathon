using Hackathon.Service.Models;

namespace Hackathon.Service.Invitations;

public interface IService
{
    // #{Student}
    Task<BasePaginationResponse> GetMyInvitations(PaginationRequest paginationRequest);
    Task<Response.InvitationItemResponse> AcceptInvitation(Guid invitationId);
    Task<Response.InvitationItemResponse> RejectInvitation(Guid invitationId);
    Task<int> GetPendingInvitationsCount();
}
