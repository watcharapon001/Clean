using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR + Pipeline Behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Common.Behaviors.ValidationBehavior<,>));
        });

        // AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(assembly);
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}

