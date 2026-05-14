using Microsoft.EntityFrameworkCore;
using RMA.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        policy =>
        {
            policy.WithOrigins("http://localhost:5286", "https://localhost:7237") // Update with your actual Blazor app URLs
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// ============================================================
//  STARTUP LOGGING - Kiểm tra DB và hiển thị thông tin server
// ============================================================
var logger = app.Logger;

// Kiểm tra kết nối Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        // Test kết nối DB
        var canConnect = await context.Database.CanConnectAsync();
        if (canConnect)
        {
            logger.LogInformation("✅ [DATABASE] Kết nối SQL Server thành công!");
            logger.LogInformation("   📦 Database: RMA_SongLinh_DB");
        }
        else
        {
            logger.LogError("❌ [DATABASE] Không thể kết nối tới SQL Server!");
        }
    }
    catch (Exception ex)
    {
        logger.LogError("❌ [DATABASE] Lỗi kết nối: {Message}", ex.Message);
    }

    // Seed Database
    DbSeeder.Seed(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseCors("AllowBlazorWasm");

app.UseAuthorization();

app.MapControllers();

// Log thông tin server sau khi khởi động
app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Urls.ToList();
    var httpUrl  = addresses.FirstOrDefault(u => u.StartsWith("http://"))  ?? "http://localhost:5299";
    var httpsUrl = addresses.FirstOrDefault(u => u.StartsWith("https://")) ?? "";

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
    Console.WriteLine("║           🚀  RMA.Server  -  ĐÃ KHỞI ĐỘNG THÀNH CÔNG   ║");
    Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"║  🌐 HTTP   : {httpUrl,-44}║");
    if (!string.IsNullOrEmpty(httpsUrl))
        Console.WriteLine($"║  🔒 HTTPS  : {httpsUrl,-44}║");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"║  📖 Swagger: {httpUrl}/openapi/v1.json{new string(' ', Math.Max(0, 9 - httpsUrl.Length))}║");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("║  💡 Mở trình duyệt và truy cập địa chỉ HTTP ở trên     ║");
    Console.WriteLine("║  🛑 Nhấn Ctrl+C để dừng server                          ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
    Console.ResetColor();
    Console.WriteLine();
});

app.Run();
