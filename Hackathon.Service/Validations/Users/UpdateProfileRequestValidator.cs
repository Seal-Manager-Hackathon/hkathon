using FluentValidation;
using Hackathon.Service.Users;

namespace Hackathon.Service.Validations.Users;

public class UpdateProfileRequestValidator : AbstractValidator<Request.UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(1, 100).WithMessage("FIRST_NAME_LENGTH_INVALID")
            .When(x => x.FirstName != null);

        RuleFor(x => x.LastName)
            .Length(1, 100).WithMessage("LAST_NAME_LENGTH_INVALID")
            .When(x => x.LastName != null);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{9,15}$").WithMessage("PHONE_NUMBER_INVALID")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("DATE_OF_BIRTH_INVALID")
            .When(x => x.DateOfBirth.HasValue);
    }
}
