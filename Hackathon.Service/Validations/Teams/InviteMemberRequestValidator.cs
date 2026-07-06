using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class InviteMemberRequestValidator : AbstractValidator<Request.InviteMemberRequest>
{
    public InviteMemberRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("EMAIL_REQUIRED")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");
    }
}
