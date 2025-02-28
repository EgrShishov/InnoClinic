using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration)
                .AddHttpClients(configuration)
                .AddMessageBrokers(configuration);
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped<IOfficeRepository, OfficeRepository>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbSettings mongoDbSettings = new MongoDbSettings();
        configuration.Bind(MongoDbSettings.SectionName, mongoDbSettings);

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton(mongoDbSettings);

        services.AddPersistence()
                .AddDbContext<OfficesDbContext>(opt =>
                    opt.UseMongoDB(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName));

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFilesHttpClient, FilesHttpClient>();

        services.AddHttpClient("files", client =>
        {
            string address = configuration["DocumentsAPI"];
            client.BaseAddress = new Uri(address);
        });

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
