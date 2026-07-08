using FluentValidation;
using Hackathon.Service.Users;

namespace Hackathon.Service.Validations.Users;

public class CreateSystemReportRequestValidator : AbstractValidator<Request.CreateSystemReportRequest>
{
    public CreateSystemReportRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("TITLE_REQUIRED");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("DESCRIPTION_REQUIRED");
    }
}
