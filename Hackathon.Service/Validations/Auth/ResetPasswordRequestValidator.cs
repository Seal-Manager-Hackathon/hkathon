using FluentValidation;
using Hackathon.Service.Auths;

namespace Hackathon.Service.Validations.Auth;

public class ResetPasswordRequestValidator : AbstractValidator<Request.ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("TOKEN_REQUIRED");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("NEW_PASSWORD_REQUIRED")
            .Length(6, 128).WithMessage("NEW_PASSWORD_LENGTH_INVALID")
            .Matches(@"[A-Z]").WithMessage("NEW_PASSWORD_UPPERCASE_REQUIRED")
            .Matches(@"[0-9]").WithMessage("NEW_PASSWORD_DIGIT_REQUIRED")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("NEW_PASSWORD_SPECIAL_CHARACTER_REQUIRED");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("CONFIRM_PASSWORD_REQUIRED")
            .Equal(x => x.NewPassword).WithMessage("CONFIRM_PASSWORD_NOT_MATCH");
    }
}
