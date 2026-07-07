using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using Hackathon.Application.Common.Interfaces;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Round;

public class Service : IRoundService
{
    private readonly IRoundRepository _roundRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRoundRepository roundRepository,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService)
    {
        _roundRepository = roundRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }

    public async Task<GetRoundsResponse> GetRounds(GetRoundsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var (items, totalCount) = await _roundRepository.SearchByEventIdAsync(
            request.EventId, request.Keyword, request.RoundNo, request.IsDisable,
            request.PageIndex, request.PageSize);

        return new GetRoundsResponse
        {
            Rounds = items.Select(r => new RoundItem
            {
                Id = r.Id,
                EventId = r.EventId,
                Name = r.Name,
                Description = r.Description,
                RoundNo = r.RoundNo,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                StartSubmission = r.StartSubmission,
                EndSubmission = r.EndSubmission,
                LimitTeam = r.LimitTeam,
                IsDisable = r.IsDisable,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task CreateRound(CreateRoundRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        // Validate thời gian round
        if (request.EndTime <= request.StartTime)
            throw new BadRequestException(ErrMsg.Round.EndTimeMustBeAfterStartTime);

        if (request.StartSubmission.HasValue && request.StartSubmission.Value < request.StartTime)
            throw new BadRequestException(ErrMsg.Round.StartSubmissionMustBeAfterStartTime);

        if (request.EndSubmission.HasValue && request.EndSubmission.Value > request.EndTime)
            throw new BadRequestException(ErrMsg.Round.EndSubmissionMustBeBeforeEndTime);

        if (request.LimitTeam.HasValue && request.LimitTeam.Value < 1)
            throw new BadRequestException(ErrMsg.Round.LimitTeamMustBeAtLeast1);

        // Check StartTime và EndTime của round phải nằm trong event time
        if (ev.StartTime.HasValue && request.StartTime < ev.StartTime.Value)
            throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);

        if (ev.EndTime.HasValue && request.EndTime > ev.EndTime.Value)
            throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);

        // StartTime của round phải >= RegisterLimitTime của event
        if (ev.RegisterLimitTime.HasValue && request.StartTime < ev.RegisterLimitTime.Value)
            throw new BadRequestException(ErrMsg.Round.StartTimeMustBeAfterRegisterLimitTime);

        // Auto-calculate RoundNo
        var maxRoundNo = await _roundRepository.GetMaxRoundNoAsync(request.EventId);
        var newRoundNo = (maxRoundNo ?? 0) + 1;

        // Check previous round endTime
        if (newRoundNo > 1)
        {
            var prevRound = await _roundRepository.GetByEventIdAndRoundNoAsync(request.EventId, newRoundNo - 1);
            if (prevRound?.EndTime.HasValue == true && request.StartTime < prevRound.EndTime.Value)
                throw new BadRequestException(ErrMsg.Round.RoundStartTimeMustBeAfterPreviousRoundEndTime);
        }

        var now = DateTimeOffset.UtcNow;

        var round = new Rounds
        {
            Id = Guid.NewGuid(),
            EventId = request.EventId,
            Name = request.Name,
            Description = request.Description,
            RoundNo = newRoundNo,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            StartSubmission = request.StartSubmission,
            EndSubmission = request.EndSubmission,
            LimitTeam = request.LimitTeam,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _roundRepository.AddAsync(round);

        // Cập nhật NumberRound của event
        ev.NumberRound = (ev.NumberRound ?? 0) + 1;
        ev.UpdatedAt = now;
        await _eventRepository.UpdateAsync(ev);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateRound(UpdateRoundRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(request.RoundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        var ev = await _eventRepository.GetByIdAsync(round.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        // Lấy giá trị để validate — ưu tiên request, nếu null lấy từ entity
        var startTime = request.StartTime ?? round.StartTime;
        var endTime = request.EndTime ?? round.EndTime;
        var limitTeam = request.LimitTeam ?? round.LimitTeam;

        if (!startTime.HasValue || !endTime.HasValue)
            throw new BadRequestException("Start Time And End Time Are Required");

        // EndTime > StartTime
        if (endTime.Value <= startTime.Value)
            throw new BadRequestException(ErrMsg.Round.EndTimeMustBeAfterStartTime);

        // LimitTeam >= 1
        if (limitTeam.HasValue && limitTeam.Value < 1)
            throw new BadRequestException(ErrMsg.Round.LimitTeamMustBeAtLeast1);

        // Round time must be within event time
        if (ev.StartTime.HasValue && startTime.Value < ev.StartTime.Value)
            throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);

        if (ev.EndTime.HasValue && endTime.Value > ev.EndTime.Value)
            throw new BadRequestException(ErrMsg.Round.RoundTimeMustBeWithinEventTime);

        // StartTime >= RegisterLimitTime of event
        if (ev.RegisterLimitTime.HasValue && startTime.Value < ev.RegisterLimitTime.Value)
            throw new BadRequestException(ErrMsg.Round.StartTimeMustBeAfterRegisterLimitTime);

        // Check previous round (RoundNo - 1): StartTime >= EndTime of previous round
        if (round.RoundNo.HasValue && round.RoundNo.Value > 1)
        {
            var prevRound = await _roundRepository.GetByEventIdAndRoundNoAsync(round.EventId, round.RoundNo.Value - 1);
            if (prevRound?.EndTime.HasValue == true && startTime.Value < prevRound.EndTime.Value)
                throw new BadRequestException(ErrMsg.Round.RoundStartTimeMustBeAfterPreviousRoundEndTime);
        }

        // Check next round (RoundNo + 1): EndTime <= StartTime of next round
        if (round.RoundNo.HasValue)
        {
            var nextRound = await _roundRepository.GetByEventIdAndRoundNoAsync(round.EventId, round.RoundNo.Value + 1);
            if (nextRound?.StartTime.HasValue == true && endTime.Value > nextRound.StartTime.Value)
                throw new BadRequestException(ErrMsg.Round.RoundEndTimeMustBeBeforeNextRoundStartTime);
        }

        var now = DateTimeOffset.UtcNow;

        // Update fields
        if (request.Name != null)
            round.Name = request.Name;
        if (request.Description != null)
            round.Description = request.Description;
        if (request.StartTime.HasValue)
            round.StartTime = request.StartTime;
        if (request.EndTime.HasValue)
            round.EndTime = request.EndTime;
        if (request.StartSubmission.HasValue)
            round.StartSubmission = request.StartSubmission;
        if (request.EndSubmission.HasValue)
            round.EndSubmission = request.EndSubmission;
        if (request.LimitTeam.HasValue)
            round.LimitTeam = request.LimitTeam;

        // Re-validate startSubmission / endSubmission after time changes
        if (round.StartSubmission.HasValue && round.StartTime.HasValue && round.StartSubmission.Value < round.StartTime.Value)
            throw new BadRequestException(ErrMsg.Round.StartSubmissionMustBeAfterStartTime);

        if (round.EndSubmission.HasValue && round.EndTime.HasValue && round.EndSubmission.Value > round.EndTime.Value)
            throw new BadRequestException(ErrMsg.Round.EndSubmissionMustBeBeforeEndTime);

        round.UpdatedAt = now;
        await _roundRepository.UpdateAsync(round);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SwapRound(SwapRoundRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var currentRound = await _roundRepository.GetByIdAsync(request.RoundId);
        if (currentRound == null || currentRound.EventId != request.EventId)
            throw new NotFoundException("Round Not Found");

        var targetRound = await _roundRepository.GetByEventIdAndRoundNoAsync(request.EventId, request.TargetRoundNo);
        if (targetRound == null)
            throw new BadRequestException(ErrMsg.Round.RoundNoNotFound);

        // Ko cho swap với chính nó
        if (currentRound.Id == targetRound.Id)
            throw new BadRequestException("Cannot Swap Round With Itself");

        var now = DateTimeOffset.UtcNow;

        // Swap RoundNo
        (currentRound.RoundNo, targetRound.RoundNo) = (targetRound.RoundNo, currentRound.RoundNo);

        // Swap StartTime, EndTime, StartSubmission, EndSubmission
        (currentRound.StartTime, targetRound.StartTime) = (targetRound.StartTime, currentRound.StartTime);
        (currentRound.EndTime, targetRound.EndTime) = (targetRound.EndTime, currentRound.EndTime);
        (currentRound.StartSubmission, targetRound.StartSubmission) = (targetRound.StartSubmission, currentRound.StartSubmission);
        (currentRound.EndSubmission, targetRound.EndSubmission) = (targetRound.EndSubmission, currentRound.EndSubmission);

        currentRound.UpdatedAt = now;
        targetRound.UpdatedAt = now;

        await _roundRepository.UpdateAsync(currentRound);
        await _roundRepository.UpdateAsync(targetRound);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int?> GetMaxRoundNo(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        return await _roundRepository.GetMaxRoundNoAsync(eventId);
    }

    public async Task DeleteRound(Guid roundId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        var ev = await _eventRepository.GetByIdAsync(round.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var deletedRoundNo = round.RoundNo ?? 0;

        // Set roundNo của round bị xóa thành 0
        round.RoundNo = 0;
        round.UpdatedAt = DateTimeOffset.UtcNow;

        // Trừ NumberRound của event
        ev.NumberRound = Math.Max(0, (ev.NumberRound ?? 0) - 1);
        ev.UpdatedAt = DateTimeOffset.UtcNow;

        // Các round có roundNo > round bị xóa → roundNo giảm 1
        var higherRounds = await _roundRepository.GetRoundsGreaterThanRoundNoAsync(round.EventId, deletedRoundNo);
        foreach (var r in higherRounds)
        {
            r.RoundNo = (r.RoundNo ?? 0) - 1;
            r.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await _roundRepository.UpdateAsync(round);
        await _eventRepository.UpdateAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreRound(Guid roundId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        // Chỉ cho restore round đã bị xóa (RoundNo = 0)
        if (round.RoundNo != 0)
            throw new BadRequestException(ErrMsg.Round.RoundNotDeleted);

        var ev = await _eventRepository.GetByIdAsync(round.EventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        var now = DateTimeOffset.UtcNow;

        // RoundNo mới = max roundNo hiện tại + 1
        var maxRoundNo = await _roundRepository.GetMaxRoundNoAsync(round.EventId);
        round.RoundNo = (maxRoundNo ?? 0) + 1;

        // Xóa thời gian (start, end, submission) khi restore
        round.StartTime = null;
        round.EndTime = null;
        round.StartSubmission = null;
        round.EndSubmission = null;

        round.UpdatedAt = now;

        // Cộng lại NumberRound của event
        ev.NumberRound = (ev.NumberRound ?? 0) + 1;
        ev.UpdatedAt = now;

        await _roundRepository.UpdateAsync(round);
        await _eventRepository.UpdateAsync(ev);
        await _unitOfWork.SaveChangesAsync();
    }
}
