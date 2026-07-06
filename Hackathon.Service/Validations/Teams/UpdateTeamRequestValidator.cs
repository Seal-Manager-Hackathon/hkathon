using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class UpdateTeamRequestValidator : AbstractValidator<Request.UpdateTeamRequest>
{
    public UpdateTeamRequestValidator()
    {
        RuleFor(x => x.TeamName)
            .NotEmpty().WithMessage("TEAM_NAME_REQUIRED");
    }
}
