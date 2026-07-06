using FluentValidation;
using Hackathon.Service.Auths;

namespace Hackathon.Service.Validations.Auth;

public class ForgotPasswordRequestValidator : AbstractValidator<Request.ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("EMAIL_REQUIRED")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");
    }
}
