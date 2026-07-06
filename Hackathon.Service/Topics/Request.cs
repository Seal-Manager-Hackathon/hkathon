using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Topics;

public static class Request
{
    public class UpdateTopicRequest
    {
        [Required(ErrorMessage = "TOPIC_TITLE_REQUIRED")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class CreateTopicRequest
    {
        [Required(ErrorMessage = "TOPIC_TITLE_REQUIRED")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
