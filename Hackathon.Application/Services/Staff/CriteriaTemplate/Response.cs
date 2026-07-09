namespace Hackathon.Application.Services.Staff.CriteriaTemplate;

public class GetCriteriaTemplateResponse
{
    public List<CriteriaTemplateItem> Items { get; set; } = new();
}

public class CriteriaTemplateItem
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetCriteriaItemsResponse
{
    public List<CriteriaItemDetail> Items { get; set; } = new();
}

public class CriteriaItemDetail
{
    public Guid Id { get; set; }
    public Guid CriteriaTemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? MaxScore { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
