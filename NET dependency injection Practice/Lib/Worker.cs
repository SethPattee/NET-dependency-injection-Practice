namespace NET_dependency_injection_Practice.Lib
{
    /* public sealed class Worker : BackgroundService
     {
         private readonly IMessageWriter _messageWriter;

         public Worker(IMessageWriter messageWriter) =>
             _messageWriter = messageWriter;

         protected override async Task ExecuteAsync(CancellationToken stoppingToken)
         {
             while (!stoppingToken.IsCancellationRequested)
             {
                 _messageWriter.Write($"Worker running at: {DateTimeOffset.Now}");
                 await Task.Delay(1_000, stoppingToken);
             }
         }
     }*/

    /* public class Worker : BackgroundService
     {
         private readonly ILogger<Worker> _logger;

         public Worker(ILogger<Worker> logger) =>
             _logger = logger;

         protected override async Task ExecuteAsync(CancellationToken stoppingToken)
         {
             while (!stoppingToken.IsCancellationRequested)
             {
                 _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                 await Task.Delay(1_000, stoppingToken);
             }
         }
     }*/

    public sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory) =>
            (_logger, _serviceScopeFactory) = (logger, serviceScopeFactory);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        _logger.LogInformation(
                            "Starting scoped work, provider hash: {hash}.",
                            scope.ServiceProvider.GetHashCode());

                        var store = scope.ServiceProvider.GetRequiredService<IObjectStore>();
                        var next = await store.GetNextAsync();
                        _logger.LogInformation("{next}", next);

                        var processor = scope.ServiceProvider.GetRequiredService<IObjectProcessor>();
                        await processor.ProcessAsync(next);
                        _logger.LogInformation("Processing {name}.", next.Name);

                        var relay = scope.ServiceProvider.GetRequiredService<IObjectRelay>();
                        await relay.RelayAsync(next);
                        _logger.LogInformation("Processed results have been relayed.");

                        var marked = await store.MarkAsync(next);
                        _logger.LogInformation("Marked as processed: {next}", marked);
                    }
                    finally
                    {
                        _logger.LogInformation(
                            "Finished scoped work, provider hash: {hash}.{nl}",
                            scope.ServiceProvider.GetHashCode(), Environment.NewLine);
                    }
                }
            }
        }
    }
}
