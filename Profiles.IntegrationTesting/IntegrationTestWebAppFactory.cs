using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("ProfilesDb")
        .WithUsername("postgres")
        .WithPassword("rootroot")
        .Build();

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
            .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ProfilesDbContext>);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ProfilesDbContext>(opt =>
            {
                opt.UseNpgsql(_dbContainer.GetConnectionString()); //provide string at runtime
            });
        });
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}
