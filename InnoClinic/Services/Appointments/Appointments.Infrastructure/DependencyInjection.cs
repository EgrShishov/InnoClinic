using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration)
                .AddEmail(configuration)
                .AddHttpClients(configuration)
                .AddMessageBroker(configuration)
                .AddCaching(configuration)
                .AddTransient<IPDFDocumentGenerator, PDFDodumentGenerator>();

        return services;
    }    
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped<IAppointmentsRepository, AppointmentsRepository>()
                .AddScoped<IAppointmentsResultRepository, AppointmentsResultRepository>()
                .AddScoped<IServiceRepository, ServiceRepository>();

        return services;
    }    
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence()
            .AddDbContext<AppointmentsDbContext>(opt =>
                opt.UseNpgsql(
                    configuration.GetConnectionString("AppointmentsDb")));

        return services;
    }

    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = new EmailSettings();
        configuration.Bind(EmailSettings.SectionName, emailSettings);

        services.AddSingleton(emailSettings)
                .AddTransient<IEmailSender, EmailSender>();

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CircuitBreakerSettings>(configuration.GetSection("CircuitBreakerSettings"))
                .Configure<RetrySettings>(configuration.GetSection("RetrySettings"));

        services.AddSingleton<ICircuitBreakerSettings, CircuitBreakerSettings>(
            sp => sp.GetRequiredService<IOptions<CircuitBreakerSettings>>().Value);

        services.AddSingleton<IRetrySettings, RetrySettings>(
            sp => sp.GetRequiredService<IOptions<RetrySettings>>().Value);

        services.AddScoped<IFilesHttpClient, FilesHttpClient>()
                .AddScoped<IAccountsHttpClient, AccountHttpClient>()
                .AddScoped<IProfilesHttpClient, ProfilesHttpClient>();

        services.AddHttpClient("files", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(configuration["FilesService"]);
        });

        services.AddHttpClient("profiles", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(configuration["ProfilesService"]);
        });

        services.AddHttpClient("identity",(serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(configuration["IdentityService"]);
        });

        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOpt =>
        {
            string connection = configuration.GetConnectionString("Redis");

            redisOpt.Configuration = connection;
        });

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }

    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetRequiredSection(MessageBrokerSettings.SectionName))
            .AddSingleton(conf => conf.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<ServiceCreatedConsumer>();
            busConfigurator.AddConsumer<ServiceStatusChangedConsumer>();
            busConfigurator.AddConsumer<ServiceUpdatedConsumer>();
            busConfigurator.AddConsumer<ServiceDeletedConsumer>();

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
