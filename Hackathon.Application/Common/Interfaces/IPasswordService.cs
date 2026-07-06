namespace Hackathon.Application.Common.Interfaces;

public interface IPasswordService
{
    string HashPassword(string rawPassword);
    bool VerifyPassword(string rawPassword, string hashedPassword);
}
