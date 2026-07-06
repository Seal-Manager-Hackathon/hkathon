namespace Hackathon.Application.Services.Team;

public class GetTeamCountRequest
{
    /// <summary>
    /// Lọc theo trạng thái disable. Không truyền = lấy tất cả.
    /// true = chỉ team bị disable, false = chỉ team không bị disable
    /// </summary>
    public bool? IsDisable { get; set; }
}
