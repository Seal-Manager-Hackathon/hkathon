namespace Hackathon.Service.Exceptions;

public class InvalidYearException : AppException
{
    public InvalidYearException()
        : base("Bad Request", 400, "INVALID_YEAR", "Năm chỉ định không hợp lệ.") { }
}

public class LeaderBoardDetailNotFoundException : AppException
{
    public LeaderBoardDetailNotFoundException()
        : base("Not Found", 404, "LEADERBOARD_DETAIL_NOT_FOUND", "Không tìm thấy thông tin xếp hạng của đội.") { }
}
