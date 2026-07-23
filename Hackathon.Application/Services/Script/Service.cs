using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Script;

public class Service : IScriptService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<BulkCreateUsersResponse> BulkCreateUsers(BulkCreateUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        if (!Enum.TryParse<RoleEnum>(request.Role, true, out var role))
            throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");

        var domain = request.EmailDomain.Trim();
        if (!domain.StartsWith("@"))
            domain = "@" + domain;

        var existingEmails = await _userRepository.GetEmailsByPrefixAsync(request.EmailPrefix, domain);
        var existingStudentIds = await _userRepository.GetStudentIdsByPrefixAsync("SE");
        var existingPhoneNumbers = await _userRepository.GetPhoneNumbersByPrefixAsync("09");

        // Parse số thứ tự từ các field đã tồn tại
        var existingNumbers = new HashSet<int>();

        void ParseNumbers(IEnumerable<string> items, string removePrefix, string? removeSuffix = null)
        {
            foreach (var item in items)
            {
                var numStr = item;
                if (!string.IsNullOrEmpty(removePrefix))
                    numStr = numStr.Replace(removePrefix, "", StringComparison.OrdinalIgnoreCase);
                if (removeSuffix != null)
                    numStr = numStr.Replace(removeSuffix, "", StringComparison.OrdinalIgnoreCase);
                // Trim leading zeros
                numStr = numStr.TrimStart('0');
                if (int.TryParse(numStr, out var num))
                    existingNumbers.Add(num);
            }
        }

        ParseNumbers(existingEmails, request.EmailPrefix, domain);
        ParseNumbers(existingStudentIds, "SE");
        ParseNumbers(existingPhoneNumbers, "09");

        var nextSeq = 1;
        while (existingNumbers.Contains(nextSeq))
            nextSeq++;

        var now = DateTimeOffset.UtcNow;
        var createdUsers = new List<Users>();

        for (var i = 0; i < request.Count; i++)
        {
            while (existingNumbers.Contains(nextSeq))
                nextSeq++;

            var seq = nextSeq;
            nextSeq++;

            var email = $"{request.EmailPrefix}{seq}{domain}";
            var firstName = $"{request.FirstName} {seq}";
            var lastName = $"{request.LastName} {seq}";
            var rawPassword = "string";
            var studentId = $"SE{seq:D6}";
            var phoneNumber = $"09{seq:D8}";

            var user = new Users
            {
                Id = Guid.NewGuid(),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                StudentId = studentId,
                HashPassword = _passwordService.HashPassword(rawPassword),
                Role = role,
                IsVerified = true,
                VerifyEmailAt = now,
                Status = UserStatusEnum.Active,
                AvatarUrl = $"https://robohash.org/{email}",
                College = "FPT University",
                CreatedAt = now,
                UpdatedAt = now
            };

            createdUsers.Add(user);
            existingNumbers.Add(seq);
        }

        foreach (var user in createdUsers)
        {
            await _userRepository.AddAsync(user);
        }
        await _unitOfWork.SaveChangesAsync();

        var totalCount = createdUsers.Count;
        var pagedItems = createdUsers
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new BulkCreateUserItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Password = "string",
                Role = u.Role.ToString(),
                StudentId = u.StudentId,
                PhoneNumber = u.PhoneNumber
            }).ToList();

        return new BulkCreateUsersResponse
        {
            Users = pagedItems,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
