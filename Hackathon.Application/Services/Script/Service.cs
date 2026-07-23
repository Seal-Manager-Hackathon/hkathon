using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Invitation;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Script;

public class Service : IScriptService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ITeamRepository _teamRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IAuthorizationService authorizationService,
        ITeamRepository teamRepository,
        IInvitationRepository invitationRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _authorizationService = authorizationService;
        _teamRepository = teamRepository;
        _invitationRepository = invitationRepository;
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

    public async Task<BulkCreateTeamResponse> BulkCreateTeam(BulkCreateTeamRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        // 1. Tìm leader user
        var leader = await _userRepository.GetByEmailAsync(request.LeaderEmail.Trim().ToLower());
        if (leader == null)
            throw new NotFoundException($"Leader With Email '{request.LeaderEmail}' Not Found");
        if (leader.IsDisable)
            throw new BadRequestException("Leader User Is Disabled");

        // 2. Check tên team không trùng
        var existingTeam = await _teamRepository.GetByNameAsync(request.TeamName);
        if (existingTeam != null)
            throw new BadRequestException("Team Name Already Exists");

        // 3. Tạo team
        var now = DateTimeOffset.UtcNow;
        var team = new Teams
        {
            Id = Guid.NewGuid(),
            Name = request.TeamName,
            CanEdit = true,
            CreatedAt = now,
            UpdatedAt = now
        };
        await _teamRepository.AddAsync(team);

        // 4. Thêm leader làm thành viên
        var leaderDetail = new TeamDetails
        {
            Id = Guid.NewGuid(),
            TeamId = team.Id,
            UserId = leader.Id,
            IsLeader = true,
            Status = TeamDetailStatusEnum.Active,
            CreatedAt = now,
            UpdatedAt = now
        };
        await _teamRepository.AddTeamDetailAsync(leaderDetail);
        await _unitOfWork.SaveChangesAsync();

        // 5. Mời và chấp nhận từng member
        var members = new List<(Users User, TeamDetails Detail)>
        {
            (leader, leaderDetail)
        };

        foreach (var memberEmail in request.MemberEmails)
        {
            var memberUser = await _userRepository.GetByEmailAsync(memberEmail.Trim().ToLower());
            if (memberUser == null)
                throw new NotFoundException($"Member With Email '{memberEmail}' Not Found");
            if (memberUser.IsDisable)
                throw new BadRequestException($"Member '{memberEmail}' Is Disabled");

            // Kiểm tra chưa là member
            var existingMembers = await _teamRepository.GetTeamMembersAsync(team.Id);
            if (existingMembers.Any(m => m.UserId == memberUser.Id && !m.IsDisable))
                throw new BadRequestException($"User '{memberEmail}' Is Already a Member of This Team");

            // Tạo invitation
            var invitation = new Invitations
            {
                Id = Guid.NewGuid(),
                TeamId = team.Id,
                UserId = memberUser.Id,
                Status = InvitationStatusEnum.Accepted,
                Description = $"System created: {leader.Email} invited {memberUser.Email} to join {team.Name}",
                LimitTime = now.AddDays(15),
                CreatedAt = now,
                UpdatedAt = now
            };
            await _invitationRepository.AddAsync(invitation);

            // Thêm thẳng member vào team (không cần chờ accept)
            var memberDetail = new TeamDetails
            {
                Id = Guid.NewGuid(),
                TeamId = team.Id,
                UserId = memberUser.Id,
                IsLeader = false,
                Status = TeamDetailStatusEnum.Active,
                CreatedAt = now,
                UpdatedAt = now
            };
            await _teamRepository.AddTeamDetailAsync(memberDetail);

            members.Add((memberUser, memberDetail));
        }

        await _unitOfWork.SaveChangesAsync();

        return new BulkCreateTeamResponse
        {
            TeamId = team.Id,
            TeamName = team.Name,
            Members = members.Select(m => new TeamMemberItem
            {
                UserId = m.User.Id,
                Email = m.User.Email,
                FirstName = m.User.FirstName,
                LastName = m.User.LastName,
                IsLeader = m.Detail.IsLeader
            }).ToList()
        };
    }
}
