using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration)
                .AddHttpClients(configuration)
                .AddMessageBroker(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped<IDoctorRepository, DoctorRepository>()
                .AddScoped<IOfficeRepository, OfficeRepository>()
                .AddScoped<IPatientRepository, PatientRepository>()
                .AddScoped<IReceptionistRepository, ReceptionistRepository>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence()
            .AddDbContext<ProfilesDbContext>(
            options => options.UseNpgsql(
                configuration.GetConnectionString("ProfilesDb")));

        return services;
    }
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountHttpClient, AccountHttpClient>();

        services.AddHttpClient("identity", client =>
        {
            client.BaseAddress = new Uri(configuration["IdentityAPI"]);
        });

        services.AddHttpClient("files", client =>
        {
            client.BaseAddress = new Uri(configuration["DocumentsAPI"]);
        });

        return services;
    }
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetRequiredSection(MessageBrokerSettings.SectionName))
            .AddSingleton(conf => conf.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<OfficeCreatedConsumer>();
            busConfigurator.AddConsumer<OfficeDeletedConsumer>();
            busConfigurator.AddConsumer<OfficeUpdatedConsumer>();
            busConfigurator.AddConsumer<OfficeStatusChangedConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();
                
                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
