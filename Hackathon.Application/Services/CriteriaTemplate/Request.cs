using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.CriteriaTemplate;

public class GetCriteriaTemplatesByRoundRequest
{
    [Required(ErrorMessage = "RoundId Is Required")]
    public Guid RoundId { get; set; }

    public string? Keyword { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}

public class GetCriteriaTemplatesByRoundResponse
{
    public List<CriteriaTemplateItem> Templates { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class CriteriaTemplateItem
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public List<CriteriaTemplateItemDetail> Items { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class CreateCriteriaTemplateRequest
{
    [Required(ErrorMessage = "RoundId Is Required")]
    public Guid RoundId { get; set; }

    [Required(ErrorMessage = "Title Is Required")]
    [StringLength(200, ErrorMessage = "Title Must Not Exceed 200 Characters")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [MinLength(1, ErrorMessage = "At Least One Criteria Item Is Required")]
    public List<CreateCriteriaItemRequest> Items { get; set; } = new();
}

public class CreateCriteriaItemRequest
{
    [Required(ErrorMessage = "Name Is Required")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Score Must Be Greater Than Or Equal To 0")]
    public decimal Score { get; set; }
}

public class UpdateCriteriaTemplateRequest
{
    public Guid TemplateId { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsDisable { get; set; }
}

public class CriteriaTemplateItemDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
