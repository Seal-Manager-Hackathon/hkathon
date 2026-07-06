using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class RegisterEventRequestValidator : AbstractValidator<Request.RegisterEventRequest>
{
    public RegisterEventRequestValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty().WithMessage("TEAM_ID_REQUIRED");

        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("EVENT_ID_REQUIRED");
    }
}
