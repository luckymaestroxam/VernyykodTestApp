using Api.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddRepositories();
builder.AddServices();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAppExceptionHandler();
app.MapControllers();
app.Run();
