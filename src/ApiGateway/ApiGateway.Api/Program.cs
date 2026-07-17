var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/user/swagger/v1/swagger.json", "User Service");
    options.SwaggerEndpoint("/finance/swagger/v1/swagger.json", "Finance Service");
});

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapReverseProxy();

app.Run();
