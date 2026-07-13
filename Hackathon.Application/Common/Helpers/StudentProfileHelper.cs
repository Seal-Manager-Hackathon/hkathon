using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Helpers;

public static class StudentProfileHelper
{
    /// <summary>
    /// Kiểm tra user đã điền đủ thông tin profile cơ bản để tham gia event chưa.
    /// Email, FirstName, LastName, College, StudentId, PhoneNumber đều phải có giá trị.
    /// </summary>
    public static void ValidateProfile(Users user)
    {
        var missingFields = new List<string>();

        if (string.IsNullOrWhiteSpace(user.Email))
            missingFields.Add("Email");
        if (string.IsNullOrWhiteSpace(user.FirstName))
            missingFields.Add("FirstName");
        if (string.IsNullOrWhiteSpace(user.LastName))
            missingFields.Add("LastName");
        if (string.IsNullOrWhiteSpace(user.College))
            missingFields.Add("College");
        if (string.IsNullOrWhiteSpace(user.StudentId))
            missingFields.Add("StudentId");
        if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            missingFields.Add("PhoneNumber");

        if (missingFields.Count > 0)
        {
            var fields = string.Join(", ", missingFields);
            throw new BadRequestException($"Please Complete Your Profile Before Proceeding. Missing Fields: {fields}");
        }
    }
}
