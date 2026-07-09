using Hackathon.Application.Services.Staff.Assign;
using Hackathon.Application.Services.Staff.CriteriaTemplate;
using Hackathon.Application.Services.Staff.Event;
using Hackathon.Application.Services.Staff.Round;
using Hackathon.Application.Services.Staff.Topic;
using Hackathon.Application.Services.Staff.Track;
using Hackathon.Application.Services.Staff.RegisterTeam;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Staff;

public static class DependencyInjection
{
    public static IServiceCollection AddStaffServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        services.AddScoped<IRoundService, Round.Service>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplate.Service>();
        services.AddScoped<ITrackService, Track.Service>();
        services.AddScoped<ITopicService, Topic.Service>();
        services.AddScoped<IAssignService, Assign.Service>();
        services.AddScoped<IRegisterTeamService, RegisterTeam.Service>();
        return services;
    }
}
