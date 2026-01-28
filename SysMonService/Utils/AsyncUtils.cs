namespace SysMonService.Utils;

public static class AsyncUtils
{
    public static async Task StartAsync(Action action, CancellationToken stoppingToken, double pollingRate = 0)
    {
        using PeriodicTimer timer = new (TimeSpan.FromMilliseconds(pollingRate));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            action();
        }
    }
}