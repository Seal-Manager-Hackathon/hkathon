using FluentValidation;
using Hackathon.Service.Staff;

namespace Hackathon.Service.Validations.Staff;

public class SearchStaffEventsRequestValidator : AbstractValidator<Request.SearchStaffEventsRequest>
{
    public SearchStaffEventsRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("PAGE_INDEX_MUST_BE_GREATER_THAN_ZERO");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO")
            .LessThanOrEqualTo(100).WithMessage("PAGE_SIZE_MUST_BE_LESS_THAN_OR_EQUAL_TO_100");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("MONTH_MUST_BE_BETWEEN_1_AND_12")
            .When(x => x.Month.HasValue);

        RuleFor(x => x.Year)
            .GreaterThan(0).WithMessage("YEAR_MUST_BE_GREATER_THAN_ZERO")
            .When(x => x.Year.HasValue);
    }
}
