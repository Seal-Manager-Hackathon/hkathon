using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class CreateTeamRequestValidator : AbstractValidator<Request.CreateTeamRequest>
{
    public CreateTeamRequestValidator()
    {
        RuleFor(x => x.TeamName)
            .NotEmpty().WithMessage("TEAM_NAME_REQUIRED");
    }
}
