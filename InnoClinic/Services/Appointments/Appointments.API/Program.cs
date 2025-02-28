using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddPresentation();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.WriteTo.File(path:"ResponseErrors.txt", outputTemplate: "{Message}{NewLine:1}{Exception:1}");
    configuration.WriteTo.Console(theme: new CustomConsoleTheme());
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
{
    options.Authority = "http://identity.api";
    options.RequireHttpsMetadata = false;
    options.Audience = "gateway-api";
});
builder.Services.AddAuthorization();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppointmentsDbContext>();

var migrations = context.Database.GetPendingMigrations();

if (migrations.Any())
{
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.UseLoggingMiddleware();

app.Run();
