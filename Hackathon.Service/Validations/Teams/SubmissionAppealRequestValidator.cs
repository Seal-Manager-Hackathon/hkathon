using FluentValidation;
using Hackathon.Service.Teams;

namespace Hackathon.Service.Validations.Teams;

public class SubmissionAppealRequestValidator : AbstractValidator<Request.SubmissionAppealRequest>
{
    public SubmissionAppealRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("TITLE_REQUIRED");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("DESCRIPTION_REQUIRED");

        RuleFor(x => x.ImgUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("IMG_URL_INVALID")
            .When(x => !string.IsNullOrWhiteSpace(x.ImgUrl));

        RuleFor(x => x.FileUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("FILE_URL_INVALID")
            .When(x => !string.IsNullOrWhiteSpace(x.FileUrl));
    }
}
