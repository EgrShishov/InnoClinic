using Auth.Application.Common;
using Auth.Infrastructure.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Auth.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton<ITokenGenerator, TokenGenerator>()
             .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(opt =>
                 opt.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtSettings.Issuer,
                     ValidAudience = jwtSettings.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(jwtSettings.Secret)
                    )
                 });

            return services;
        }

        public static IServiceCollection AddPersistense(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AuthDbContext>(
                       options =>
                       options.UseNpgsql(
                               configuration.GetConnectionString("AuthorizationDb")));
            return services;
        }
    }
}
