using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration)
                .AddPresentation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});

Config.InitializeJwtSettings(builder.Configuration);

builder.Services.AddIdentityServer(opt =>
{
    opt.EmitStaticAudienceClaim = true;
})
.AddInMemoryClients(Config.GetClients())
.AddInMemoryIdentityResources(Config.GetIdentityResources())
.AddInMemoryApiScopes(Config.GetApiScopes())
.AddAspNetIdentity<Account>()
.AddDeveloperSigningCredential();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();

await RoleSeeder.SeedRolesAsync(roleManager);

await BaseReceptionistAccountSeeder.SeedBaseReceptionistAccountAsync(roleManager, userManager);

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseIdentityServer();

app.Run();
