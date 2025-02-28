using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IBlobStorageService, BlobStorageService>()
                .AddScoped<IPhotoRepository, PhotoRepository>();

        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")))
                .AddPersistence()
                .AddDbContext<DocumentsDbContext>(
                    options => 
                        options.UseNpgsql(
                            configuration.GetConnectionString("DocumentsDb")));

        return services;
    }
}