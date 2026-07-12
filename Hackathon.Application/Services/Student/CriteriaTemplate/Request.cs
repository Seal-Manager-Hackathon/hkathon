namespace Hackathon.Application.Services.Student.CriteriaTemplate;

public class GetCriteriaTemplatesByRoundRequest
{
    public Guid RoundId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetCriteriaItemsByTemplateRequest
{
    public Guid TemplateId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
