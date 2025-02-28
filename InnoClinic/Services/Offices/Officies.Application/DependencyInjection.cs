using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf => {
            conf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            conf.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        })
            .AddValidation();
        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
