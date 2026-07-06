using FluentValidation;
using Hackathon.Service.Users;

namespace Hackathon.Service.Validations.Users;

public class UpdateAvatarRequestValidator : AbstractValidator<Request.UpdateAvatarRequest>
{
    public UpdateAvatarRequestValidator()
    {
        RuleFor(x => x.AvatarUrl)
            .NotNull().WithMessage("AVATAR_FILE_REQUIRED");
    }
}
