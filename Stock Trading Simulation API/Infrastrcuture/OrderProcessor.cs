using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;
using System.Collections.Concurrent;

namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class OrderProcessor
    {
        private readonly ConcurrentQueue<Order> _incomingOrders = new();
        private readonly IMatchingEngine _matchingEngine;
        private readonly IOrderBook _orderBook;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public OrderProcessor(IMatchingEngine matchingEngine, IOrderBook orderBook)
        {
            _matchingEngine = matchingEngine;
            _orderBook = orderBook;

            Console.WriteLine($"OrderProcessor CREATED: {this.GetHashCode()}");
        }

        // Enqueue new order
        public void EnqueueOrder(Order order)
        {
            Console.WriteLine($"ENQUEUED: {order.Id} | Processor: {this.GetHashCode()}");
            _incomingOrders.Enqueue(order);
        }

        // Background consumer (THIS MUST BE CALLED BY EngineWorker)
        public async Task ProcessOrdersAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"PROCESS LOOP STARTED: {this.GetHashCode()}");

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_incomingOrders.TryDequeue(out var order))
                {
                    Console.WriteLine($"DEQUEUED: {order.Id}");

                    await _semaphore.WaitAsync(cancellationToken);

                    try
                    {
                        await _orderBook.AddOrderAsync(order);
                        Console.WriteLine($"ADDED TO ORDERBOOK: {order.Id}");

                        var trades = await _matchingEngine.MatchAsync();

                        if (trades != null && trades.Count > 0)
                        {
                            Console.WriteLine($"TRADES: {trades.Count} for {order.Symbol}");
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                else
                {
                    await Task.Delay(50, cancellationToken);
                }
            }
        }
    }
}