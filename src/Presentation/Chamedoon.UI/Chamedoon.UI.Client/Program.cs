using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazoredModal();
await builder.Build().RunAsync();
