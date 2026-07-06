using FluentValidation;
using Hackathon.Service.Submissions;

namespace Hackathon.Service.Validations.Submissions;

public class SubmitRoundProjectRequestValidator : AbstractValidator<Request.SubmitRoundProjectRequest>
{
    public SubmitRoundProjectRequestValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL_REQUIRED")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("INVALID_URL_FORMAT");
    }
}
