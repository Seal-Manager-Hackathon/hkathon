using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Award;

public class GetAwardsRequest
{
    [Required(ErrorMessage = "EventId Is Required")]
    public Guid EventId { get; set; }

    public string? Keyword { get; set; }

    public bool? IsDisable { get; set; }

    public int PageIndex { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}

public class CreateAwardRequest
{
    [Required(ErrorMessage = "EventId Is Required")]
    public Guid EventId { get; set; }

    [Required(ErrorMessage = "Name Is Required")]
    [StringLength(200, ErrorMessage = "Name Must Not Exceed 200 Characters")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "NumberOfAward Must Be Greater Than 0")]
    public int NumberOfAward { get; set; } = 1;

    [Range(0.01, double.MaxValue, ErrorMessage = "Prize Must Be Greater Than 0")]
    public decimal Prize { get; set; }
}

public class UpdateAwardRequest
{
    public Guid EventId { get; set; }
    public Guid AwardId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "NumberOfAward Must Be Greater Than 0")]
    public int? NumberOfAward { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Prize Must Be Greater Than 0")]
    public decimal? Prize { get; set; }

    public bool? IsDisable { get; set; }
}
