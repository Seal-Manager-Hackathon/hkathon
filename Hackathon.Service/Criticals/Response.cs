namespace Hackathon.Service.Criticals;

public static class Response
{
    public class RoundCriteriaResponse
    {
        public Guid RoundId { get; set; }
        public Guid EventId { get; set; }
        public string RoundName { get; set; } = null!;
        public CriteriaTemplateResponse? Template { get; set; }
    }

    public class CriteriaTemplateResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<CriteriaItemResponse> Items { get; set; } = new();
    }

    public class CriteriaItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Score { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateCriteriaResponse
    {
        public Guid Id { get; set; }
    }
}
