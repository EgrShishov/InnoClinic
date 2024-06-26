using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Officies.Infrastructure.Persistence;
using Officies.Infrastructure.Repository;

namespace Officies.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            MongoDbSettings mongoDbSettings = new();
            configuration.Bind(MongoDbSettings.SectionName, mongoDbSettings);

            services.Configure<MongoDbSettings>(configuration.GetSection("OfficesDatabase"));
            services.AddInfrastructure()
                .AddSingleton<IMongoClient, MongoClient>(sp =>
                {
                    return new MongoClient(mongoDbSettings.ConnectionString);
                });

            return services;
        }
    }
}
