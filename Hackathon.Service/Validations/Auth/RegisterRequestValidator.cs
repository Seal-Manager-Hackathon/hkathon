using FluentValidation;
using Hackathon.Service.Auths;

namespace Hackathon.Service.Validations.Auth;

public class RegisterRequestValidator : AbstractValidator<Request.RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FIRST_NAME_REQUIRED")
            .Length(1, 100).WithMessage("FIRST_NAME_LENGTH_INVALID");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LAST_NAME_REQUIRED")
            .Length(1, 100).WithMessage("LAST_NAME_LENGTH_INVALID");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("EMAIL_REQUIRED")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("PASSWORD_REQUIRED");
        //     .Length(6, 128).WithMessage("PASSWORD_LENGTH_INVALID")
        //     .Matches(@"[A-Z]").WithMessage("PASSWORD_UPPERCASE_REQUIRED")
        //     .Matches(@"[0-9]").WithMessage("PASSWORD_DIGIT_REQUIRED")
        //     .Matches(@"[^a-zA-Z0-9]").WithMessage("PASSWORD_SPECIAL_CHARACTER_REQUIRED");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("CONFIRM_PASSWORD_REQUIRED")
            .Equal(x => x.Password).WithMessage("CONFIRM_PASSWORD_NOT_MATCH");
    }
}
