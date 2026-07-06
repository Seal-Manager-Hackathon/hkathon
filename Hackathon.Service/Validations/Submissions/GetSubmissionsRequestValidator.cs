using FluentValidation;
using Hackathon.Service.Submissions;

namespace Hackathon.Service.Validations.Submissions;

public class GetSubmissionsRequestValidator : AbstractValidator<Request.GetSubmissionsRequest>
{
    public GetSubmissionsRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("PAGE_INDEX_MUST_BE_GREATER_THAN_ZERO");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO")
            .LessThanOrEqualTo(100).WithMessage("PAGE_SIZE_MUST_BE_LESS_THAN_OR_EQUAL_TO_100");
    }
}
