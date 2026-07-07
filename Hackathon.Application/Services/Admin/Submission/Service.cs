using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Submission;

public class Service : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ISubmissionRepository submissionRepository,
        IEventRepository eventRepository,
        IRoundRepository roundRepository,
        IRegisterTeamRepository registerTeamRepository,
        ITrackRepository trackRepository,
        IAuthorizationService authorizationService)
    {
        _submissionRepository = submissionRepository;
        _eventRepository = eventRepository;
        _roundRepository = roundRepository;
        _registerTeamRepository = registerTeamRepository;
        _trackRepository = trackRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetSubmissionsResponse> GetSubmissions(GetSubmissionsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            request.EventId, request.RoundId, request.TrackId,
            request.TopicId, request.RegisterTeamId, request.Keyword,
            request.PageIndex, request.PageSize);

        return new GetSubmissionsResponse
        {
            Items = items.Select(rd => new SubmissionItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                EventId = rd.RegisterTeam.EventId,
                EventName = rd.RegisterTeam.Event.Name,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                SubmittedBy = rd.RegisterTeam.Team.TeamDetails
                    .Where(td => td.IsLeader)
                    .Select(td => new SubmittedByUser
                    {
                        UserId = td.UserId,
                        Email = td.User.Email,
                        FirstName = td.User.FirstName,
                        LastName = td.User.LastName
                    })
                    .FirstOrDefault(),
                LastSubmission = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .FirstOrDefault(),
                Records = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .ToList()
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetSubmissionsResponse> GetSubmissionsByRound(Guid roundId, string? keyword, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            round.EventId, roundId, null, null, null, keyword,
            pageIndex, pageSize);

        return new GetSubmissionsResponse
        {
            Items = items.Select(rd => new SubmissionItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                EventId = rd.RegisterTeam.EventId,
                EventName = rd.RegisterTeam.Event.Name,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                SubmittedBy = rd.RegisterTeam.Team.TeamDetails
                    .Where(td => td.IsLeader)
                    .Select(td => new SubmittedByUser
                    {
                        UserId = td.UserId,
                        Email = td.User.Email,
                        FirstName = td.User.FirstName,
                        LastName = td.User.LastName
                    })
                    .FirstOrDefault(),
                LastSubmission = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .FirstOrDefault(),
                Records = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .ToList()
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetSubmissionsResponse> GetSubmissionsByRegisterTeam(Guid registerTeamId, Guid? roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var registerTeam = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            registerTeam.EventId, roundId, null, null, registerTeamId, null,
            pageIndex, pageSize);

        return new GetSubmissionsResponse
        {
            Items = items.Select(rd => new SubmissionItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                EventId = rd.RegisterTeam.EventId,
                EventName = rd.RegisterTeam.Event.Name,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                SubmittedBy = rd.RegisterTeam.Team.TeamDetails
                    .Where(td => td.IsLeader)
                    .Select(td => new SubmittedByUser
                    {
                        UserId = td.UserId,
                        Email = td.User.Email,
                        FirstName = td.User.FirstName,
                        LastName = td.User.LastName
                    })
                    .FirstOrDefault(),
                LastSubmission = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .FirstOrDefault(),
                Records = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .ToList()
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetSubmissionsResponse> GetSubmissionsByTrack(Guid trackId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            track.EventId, null, trackId, null, null, null,
            pageIndex, pageSize);

        return new GetSubmissionsResponse
        {
            Items = items.Select(rd => new SubmissionItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                EventId = rd.RegisterTeam.EventId,
                EventName = rd.RegisterTeam.Event.Name,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                SubmittedBy = rd.RegisterTeam.Team.TeamDetails
                    .Where(td => td.IsLeader)
                    .Select(td => new SubmittedByUser
                    {
                        UserId = td.UserId,
                        Email = td.User.Email,
                        FirstName = td.User.FirstName,
                        LastName = td.User.LastName
                    })
                    .FirstOrDefault(),
                LastSubmission = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .FirstOrDefault(),
                Records = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .ToList()
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}
