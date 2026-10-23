using Cis.Infrastructure.Ioc.DependencyInjection;
using Cis.Presentation.Controllers;
using global::Cis.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(TopicQueryController).Assembly);

builder.Services.AddOpenApi();
builder.Services.AddCisInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
