using FluentValidation;
using Hackathon.Service.Auths;

namespace Hackathon.Service.Validations.Auth;

public class ResendEmailVerificationRequestValidator : AbstractValidator<Request.ResendEmailVerificationRequest>
{
    public ResendEmailVerificationRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("EMAIL_REQUIRED")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");
    }
}
