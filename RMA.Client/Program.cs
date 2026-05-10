using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RMA.Client;
using RMA.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5299") }); // Connect to Server API
builder.Services.AddMudServices();

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<RmaTicketService>();

await builder.Build().RunAsync();
