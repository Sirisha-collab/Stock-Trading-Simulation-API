namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class BackgroundOrderService : BackgroundService
    {
        private readonly OrderProcessor _processor;

        public BackgroundOrderService(OrderProcessor processor)
        {
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _processor.ProcessOrdersAsync(stoppingToken);
        }
    }
}
