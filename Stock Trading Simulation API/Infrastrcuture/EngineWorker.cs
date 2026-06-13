using Stock_Trading_Simulation_API.Infrastrcuture;

namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class EngineWorker: BackgroundService
    {
        private readonly OrderProcessor _processor;
        public EngineWorker(OrderProcessor processor)
        {
            Console.WriteLine("EngineWorker CREATED");
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine(" Order Processor Started");

            await _processor.ProcessOrdersAsync(stoppingToken);
        }
    }
}

