var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfratructure(builder.Configuration)
                .AddApplication()
                .AddPresentation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();

var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

dbInitializer.Initialize();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
