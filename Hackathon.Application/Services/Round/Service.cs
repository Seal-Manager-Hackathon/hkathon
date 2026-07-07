using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using Hackathon.Application.Common.Interfaces;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Round;

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
            request.EventId, request.Keyword, request.RoundNo,
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
}
