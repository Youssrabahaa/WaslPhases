using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using phase_1.BLL.Services;

namespace phase_1.BLL.BackgroundJobs
{
    public class ReminderDispatcherHostedService : BackgroundService
    {
        private static readonly TimeSpan PollingInterval = TimeSpan.FromMinutes(1);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReminderDispatcherHostedService> _logger;

        public ReminderDispatcherHostedService(IServiceScopeFactory scopeFactory, ILogger<ReminderDispatcherHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(PollingInterval);

            do
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
                    await reminderService.ProcessDueRemindersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process due session reminders.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
