using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Round;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Round;

public class Service : IRoundService
{
    private readonly IRoundRepository _roundRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IRoundRepository roundRepository,
        IEventRepository eventRepository,
        IAuthorizationService authorizationService)
    {
        _roundRepository = roundRepository;
        _eventRepository = eventRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRoundsResponse> GetRounds(Guid eventId, string? keyword, int? roundNo)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var ev = await _eventRepository.GetByIdAsync(eventId);
        if (ev == null)
            throw new NotFoundException("Event Not Found");

        // Lecturer: luôn lọc IsDisable = false, không cho filter IsDisable
        var (items, totalCount) = await _roundRepository.SearchByEventIdAsync(
            eventId, keyword, roundNo, false,
            1, int.MaxValue);

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
            PageIndex = 1,
            PageSize = totalCount > 0 ? totalCount : 10
        };
    }
}
