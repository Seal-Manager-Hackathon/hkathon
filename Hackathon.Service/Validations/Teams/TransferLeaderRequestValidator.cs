using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class TransferLeaderRequestValidator : AbstractValidator<Request.TransferLeaderRequest>
{
    public TransferLeaderRequestValidator()
    {
        RuleFor(x => x.NewLeaderId)
            .NotEmpty().WithMessage("NEW_LEADER_ID_REQUIRED");
    }
}
