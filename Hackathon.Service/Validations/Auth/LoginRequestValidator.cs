using FluentValidation;
using Hackathon.Service.Auths;

namespace Hackathon.Service.Validations.Auth;

public class LoginRequestValidator : AbstractValidator<Request.LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("EMAIL_REQUIRED")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("PASSWORD_REQUIRED");
    }
}
