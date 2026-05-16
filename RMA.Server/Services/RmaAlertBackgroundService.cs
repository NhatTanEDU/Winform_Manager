namespace RMA.Server.Services;

/// <summary>
/// Background Service chạy ngầm.
/// TẠM THỜI VÔ HIỆU HÓA logic SQL Server để dự án có thể Build.
/// Sẽ được cập nhật để sử dụng Firestore trong bước tiếp theo.
/// </summary>
public class RmaAlertBackgroundService : BackgroundService
{
    private readonly IFcmService _fcmService;
    private readonly IConfiguration _config;
    private readonly ILogger<RmaAlertBackgroundService> _logger;

    public RmaAlertBackgroundService(
        IFcmService fcmService,
        IConfiguration config,
        ILogger<RmaAlertBackgroundService> logger)
    {
        _fcmService   = fcmService;
        _config       = config;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🕒 [RmaAlert] Background Service đã khởi động (Chế độ chờ Firestore).");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            // Logic quét Firestore sẽ được viết tại đây
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
