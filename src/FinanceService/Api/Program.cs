using FinanceService.Api.Startup;
using Shared.Api.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddRepositories();
builder.AddServices();
builder.AddAuth();
builder.Services.AddControllers();

var app = builder.Build();
app.UseAppExceptionHandler();
app.MapControllers();
app.Run();
