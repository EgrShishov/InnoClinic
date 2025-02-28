using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration)
                .AddApplication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseAuthorization();

app.Run();
