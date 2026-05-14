using Microsoft.EntityFrameworkCore;
using RMA.Server.Data;

namespace RMA.Server.Services;

/// <summary>
/// Background Service chạy ngầm, tự động kiểm tra phiếu RMA vi phạm mốc 14 ngày.
/// Gửi Push Notification qua FCM khi phát hiện vi phạm.
/// 
/// Quy tắc cảnh báo ĐỎ (theo UC03):
///   - TH A: Đã gửi máy đi hãng (SentDate != null) nhưng đã vượt 14 ngày chưa về
///   - TH B: Nhận máy từ khách (ReceivedDate) nhưng 14 ngày vẫn chưa gửi đi (SentDate == null)
/// </summary>
public class RmaAlertBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IFcmService _fcmService;
    private readonly IConfiguration _config;
    private readonly ILogger<RmaAlertBackgroundService> _logger;

    public RmaAlertBackgroundService(
        IServiceScopeFactory scopeFactory,
        IFcmService fcmService,
        IConfiguration config,
        ILogger<RmaAlertBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _fcmService   = fcmService;
        _config       = config;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int intervalMinutes = int.TryParse(_config["Firebase:CheckIntervalMinutes"], out int parsed)
            ? parsed : 60;

        _logger.LogInformation(
            "🕒 [RmaAlert] Background Service đã khởi động. Chu kỳ kiểm tra: {Interval} phút.",
            intervalMinutes);

        // Chạy lần đầu ngay khi server khởi động (delay 30s để DB sẵn sàng)
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckOverdueTicketsAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
        }
    }

    private async Task CheckOverdueTicketsAsync(CancellationToken stoppingToken)
    {
        // Nếu Firebase chưa được cấu hình, bỏ qua hoàn toàn — không query DB, không gây log rác
        if (!_fcmService.IsReady)
        {
            _logger.LogInformation("[RmaAlert] Firebase chưa sẵn sàng — bỏ qua lượt quét này.");
            return;
        }

        _logger.LogInformation(
            "🔍 [RmaAlert] Đang quét phiếu RMA quá hạn lúc: {Time}",
            DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm:ss"));

        try
        {
            // IServiceScopeFactory là bắt buộc vì BackgroundService là Singleton,
            // còn DbContext là Scoped — phải tạo scope mới để lấy DbContext
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var cutoffDate = DateTime.UtcNow.AddDays(-14);

            // Query phiếu vi phạm mốc 14 ngày và chưa đóng
            var overdueTickets = await db.RmaTickets
                .Include(t => t.Customer)
                .Include(t => t.StatusMaster)   // ⚠️ Đúng tên Navigation Property
                .Where(t =>
                    t.StatusMaster.StatusName != "Closed" &&
                    t.StatusMaster.StatusName != "Done"   &&
                    (
                        // TH A: Đã gửi đi hãng nhưng vượt 14 ngày vẫn chưa đóng ticket
                        (t.SentDate != null && t.SentDate < cutoffDate)
                        ||
                        // TH B: Nhận máy nhưng 14 ngày vẫn chưa gửi đi hãng
                        (t.SentDate == null && t.ReceivedDate < cutoffDate)
                    )
                )
                .AsNoTracking()
                .ToListAsync(stoppingToken);

            if (!overdueTickets.Any())
            {
                _logger.LogInformation("✅ [RmaAlert] Không có phiếu nào vi phạm mốc 14 ngày.");
                return;
            }

            _logger.LogWarning(
                "⚠️ [RmaAlert] Phát hiện {Count} phiếu vi phạm. Đang gửi cảnh báo FCM...",
                overdueTickets.Count);

            foreach (var ticket in overdueTickets)
            {
                string reason = ticket.SentDate == null
                    ? "Nhận máy nhưng chưa gửi đi hãng"
                    : "Đã gửi đi hãng nhưng vượt 14 ngày chưa về";

                await _fcmService.SendAlertAsync(
                    ticket.Id,
                    ticket.Customer?.Name ?? "Không rõ",
                    reason);
            }

            _logger.LogInformation(
                "📤 [RmaAlert] Đã gửi {Count} cảnh báo FCM thành công.",
                overdueTickets.Count);
        }
        catch (OperationCanceledException)
        {
            // Server đang shutdown — bình thường, không cần log lỗi
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "❌ [RmaAlert] Lỗi khi quét phiếu RMA quá hạn: {Message}", ex.Message);
        }
    }
}
