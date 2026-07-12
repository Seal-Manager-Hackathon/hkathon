namespace Hackathon.Application.Services.Student.CriteriaTemplate;

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
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetCriteriaItemsByTemplateResponse
{
    public List<CriteriaItemInfo> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class CriteriaItemInfo
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
