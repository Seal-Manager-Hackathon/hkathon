using FluentValidation;
using Hackathon.Repository.Enum;
using Hackathon.Service.Lecturers;

namespace Hackathon.Service.Validations.Lecturers;

public class SearchLecturerEventsRequestValidator : AbstractValidator<Request.SearchLecturerEventsRequest>
{
    public SearchLecturerEventsRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("PAGE_INDEX_MUST_BE_GREATER_THAN_ZERO");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO")
            .LessThanOrEqualTo(100).WithMessage("PAGE_SIZE_MUST_BE_LESS_THAN_OR_EQUAL_TO_100");

        RuleFor(x => x.EventRole)
            .Must(role => role == EventRoleEnum.Mentor || role == EventRoleEnum.Judge)
                .WithMessage("INVALID_EVENT_ROLE")
            .When(x => x.EventRole.HasValue);
    }
}
