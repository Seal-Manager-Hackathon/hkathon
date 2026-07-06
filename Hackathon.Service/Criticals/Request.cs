namespace Hackathon.Service.Criticals;

public static class Request
{
    public class CreateCriteriaRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public List<CreateCriteriaItemRequest> Items { get; set; } = new();
    }

    public class CreateCriteriaItemRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Score { get; set; }
    }
}
