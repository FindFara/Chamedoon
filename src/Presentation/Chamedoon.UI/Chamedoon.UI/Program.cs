using Blazored.Modal;
using Chamedoon.UI.Client.Pages;
using Chamedoon.UI.Components;
using Chamedoon.Application;
using Chamedoon.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddBlazorUIServices();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/octet-stream",
        "application/wasm",
        "application/javascript",
        "application/json"
    });
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseResponseCompression();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        var headers = context.Context.Response.Headers;
        if (!headers.ContainsKey("Cache-Control"))
        {
            headers.Append("Cache-Control", "public,max-age=604800");
        }
    }
});
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Chamedoon.UI.Client._Imports).Assembly);

app.Run();
