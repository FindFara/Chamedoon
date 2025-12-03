using Chamedoon.Application;
using Chamedoon.Domin.Configs;
using Chamedoon.Infrastructure;
using ChamedoonWebUI;
using Chamedoon.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddWebUIServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<UrlsConfig>(builder.Configuration.GetSection("Urls"));


var app = builder.Build();

await app.SeedIdentityDataAsync();
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
