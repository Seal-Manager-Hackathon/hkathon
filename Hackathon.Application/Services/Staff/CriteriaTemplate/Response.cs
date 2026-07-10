namespace Hackathon.Application.Services.Staff.CriteriaTemplate;

public class GetCriteriaTemplateResponse
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
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetCriteriaItemsResponse
{
    public List<CriteriaItemDetail> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class CriteriaItemDetail
{
    public Guid Id { get; set; }
    public Guid CriteriaTemplateId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetCriteriaTemplateDetailResponse
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public bool IsActive { get; set; }
    public List<CriteriaTemplateItemDetail> Items { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetCriteriaItemDetailResponse
{
    public Guid Id { get; set; }
    public Guid CriteriaTemplateId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
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
