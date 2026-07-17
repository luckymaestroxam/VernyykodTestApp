using Shared.Api.Startup;
using UserService.Api.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddRepositories();
builder.AddServices();
builder.AddAuth();
builder.AddAppSwagger();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseAppExceptionHandler();
app.MapControllers();
app.Run();
