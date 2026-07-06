using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class RemoveMembersRequestValidator : AbstractValidator<Request.RemoveMembersRequest>
{
    public RemoveMembersRequestValidator()
    {
        RuleFor(x => x.UserIds)
            .NotEmpty().WithMessage("USER_IDS_REQUIRED");
    }
}
