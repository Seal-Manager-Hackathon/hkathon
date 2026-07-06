using FluentValidation;
using Hackathon.Service.Auths;

namespace Hackathon.Service.Validations.Auth;

public class VerifyEmailRequestValidator : AbstractValidator<Request.VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("TOKEN_REQUIRED");
    }
}
