using Chamedoon.Application;
using Chamedoon.Infrastructure;
using Chamedoon.WebAPI;
using Chamedoon.Infrastructure.Persistence;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    //options.Filters.Add(new LoggingAttribute(new Logger<LoggingAttribute>(null)));
});
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebAPIServices(builder.Configuration);
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
var app = builder.Build();

await app.SeedIdentityDataAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseEndpoints(endpoints =>
{
    // area-based routing
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=UserManagement}/{action=Index}/{id?}"
    );

    // conventional routing
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
