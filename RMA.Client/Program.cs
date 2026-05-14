using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using RMA.Client;
using RMA.Client.Auth;
using RMA.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5299") }); // Connect to Server API
builder.Services.AddMudServices();

// Setup mock authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, MockAuthStateProvider>();

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<RmaTicketService>();

var host = builder.Build();

// ============================================================
//  STARTUP LOGGING - Hiển thị thông tin client
// ============================================================
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
Console.WriteLine("║          🎨  RMA.Client  -  ĐÃ KHỞI ĐỘNG THÀNH CÔNG    ║");
Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("║  🌐 Frontend  : http://localhost:5286                    ║");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("║  🔗 Backend   : http://localhost:5299                    ║");
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("║  💡 Mở Chrome/Edge và truy cập: http://localhost:5286   ║");
Console.WriteLine("║  🛑 Nhấn Ctrl+C để dừng client                          ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
Console.ResetColor();
Console.WriteLine();

await host.RunAsync();
