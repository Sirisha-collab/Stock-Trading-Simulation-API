namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class EngineWorker: BackgroundService
    {
        private readonly MatchingEngine _engine;
        public EngineWorker(MatchingEngine engine)
        {
            _engine = engine;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("🚀 Matching Engine Started");

            await _engine.Run(stoppingToken);
        }
    }
}
