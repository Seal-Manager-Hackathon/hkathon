using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Common.Interfaces;

public interface IAuthorizationService
{
    /// <summary>
    /// Kiểm tra user hiện tại đã authenticated chưa.
    /// Ném UnauthorizedException nếu chưa đăng nhập.
    /// </summary>
    void Authenticate();

    /// <summary>
    /// Kiểm tra user hiện tại có role trong danh sách được phép không.
    /// Ném UnauthorizedException nếu chưa auth, ForbiddenException nếu không đủ quyền.
    /// </summary>
    void Authorize(params RoleEnum[] allowedRoles);
}
