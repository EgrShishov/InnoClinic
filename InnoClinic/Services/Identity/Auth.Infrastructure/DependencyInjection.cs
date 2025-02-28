using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistense(configuration)
                .AddEmailService(configuration)
                .AddHttpClients(configuration)
                .AddIdentity();

        return services;
    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = new EmailSettings();
        configuration.Bind(EmailSettings.SectionName, emailSettings);

        services.AddSingleton(emailSettings);
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }

    public static IServiceCollection AddPersistense(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(
                        options =>
                            options.UseNpgsql(
                                configuration.GetConnectionString("AuthorizationDb")));
        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<Account, AppRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddUserManager<UserManager<Account>>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFilesHttpClient, FilesHttpClient>();

        services.AddHttpClient("files", (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(configuration["FilesService"]);
        });

        return services;
    }
}
