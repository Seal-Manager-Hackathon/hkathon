using Hackathon.Application.Services.Student.CriteriaTemplate;
using Hackathon.Application.Services.Student.Event;
using Hackathon.Application.Services.Student.RegisterTeam;
using Hackathon.Application.Services.Student.Round;
using Hackathon.Application.Services.Student.Topic;
using Hackathon.Application.Services.Student.Track;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Student;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        services.AddScoped<IRoundService, Round.Service>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplate.Service>();
        services.AddScoped<ITrackService, Track.Service>();
        services.AddScoped<ITopicService, Topic.Service>();
        services.AddScoped<IRegisterTeamService, RegisterTeam.Service>();
        return services;
    }
}
