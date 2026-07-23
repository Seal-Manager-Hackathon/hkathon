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

        var prefix = request.StudentIdPrefix.Trim().ToUpper();
        if (prefix.Length != 2)
            throw new BadRequestException("StudentIdPrefix Must Be Exactly 2 Characters");
        // Chỉ check email theo prefix để tìm số thứ tự
        var existingEmails = await _userRepository.GetEmailsByPrefixAsync(request.EmailPrefix, domain);
        var existingStudentIdSet = (await _userRepository.GetStudentIdsByPrefixAsync(prefix)).ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Tìm các số email đã dùng theo cùng prefix
        var usedEmailNumbers = new HashSet<int>();
        foreach (var email in existingEmails)
        {
            var numStr = email
                .Replace(request.EmailPrefix, "", StringComparison.OrdinalIgnoreCase)
                .Replace(domain, "", StringComparison.OrdinalIgnoreCase);
            if (int.TryParse(numStr, out var num))
                usedEmailNumbers.Add(num);
        }

        var now = DateTimeOffset.UtcNow;
        var createdUsers = new List<Users>();

        for (var i = 0; i < request.Count; i++)
        {
            // Tìm số email bé nhất chưa dùng
            var emailSeq = 1;
            while (usedEmailNumbers.Contains(emailSeq))
                emailSeq++;

            var email = $"{request.EmailPrefix}{emailSeq}{domain}";
            var firstName = $"{request.FirstName} {emailSeq}";
            var lastName = $"{request.LastName} {emailSeq}";
            var rawPassword = "string";

            // StudentId: tìm số bé nhất từ emailSeq trở đi chưa bị trùng
            var sidSeq = emailSeq;
            var studentId = $"{prefix}{sidSeq:D6}";
            while (existingStudentIdSet.Contains(studentId))
            {
                sidSeq++;
                studentId = $"{prefix}{sidSeq:D6}";
            }
            var phoneNumber = $"09{sidSeq:D8}";

            usedEmailNumbers.Add(emailSeq);
            existingStudentIdSet.Add(studentId);

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
            usedEmailNumbers.Add(emailSeq);
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
