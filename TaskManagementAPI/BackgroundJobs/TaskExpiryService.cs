    using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.BackgroundJobs;


public class TaskExpiryService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TaskExpiryService> _logger;

    public TaskExpiryService(
        IServiceScopeFactory scopeFactory,
        ILogger<TaskExpiryService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Task expiry background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ExpireOverdueTasksAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error while expiring overdue tasks.");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task ExpireOverdueTasksAsync(CancellationToken token)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

        var overdue = await repository.GetOverduePendingAsync(DateTime.Now);
        if (overdue.Count == 0)
            return;

        foreach (var task in overdue)
        {
            task.Status = TaskItemStatus.Expired;
            task.UpdatedAt = DateTime.Now;
            await repository.UpdateAsync(task);
        }

        _logger.LogInformation("Expired {Count} overdue task(s).", overdue.Count);
    }
}
