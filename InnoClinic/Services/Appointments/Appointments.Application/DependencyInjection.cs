using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf => 
            {
                conf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                conf.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            })
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ITimeSlotsGenerator, TimeSlotsGenerator>();
        return services;
    }
}