using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class DependencyInjection
{
    public static IServiceCollection AddInfratructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence()
                .AddMessageBrokers(configuration);

        return services;
    } 
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<DatabaseInitializer>()
                .AddScoped<IServicesRepository, ServicesRepository>()
                .AddScoped<ISpecializationsRepository, SpecializationsRepository>();
            
        return services;
    }
    public static IServiceCollection AddMessageBrokers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetRequiredSection(MessageBrokerSettings.SectionName))
                .AddSingleton(conf => conf.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, config) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

                config.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                config.ConfigureEndpoints(context);
            });
        });

        services.AddTransient<IEventBus, EventBus>();

        return services;
    }
}
