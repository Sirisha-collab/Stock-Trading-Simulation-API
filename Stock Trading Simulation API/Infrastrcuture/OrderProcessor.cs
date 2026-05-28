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
        private readonly SemaphoreSlim _semaphore = new(1, 1); // controls processing access

        public OrderProcessor(IMatchingEngine matchingEngine, IOrderBook orderBook)
        {
            _matchingEngine = matchingEngine;
            _orderBook = orderBook;
        }

        // Enqueue new order
        public void EnqueueOrder(Order order)
        {
            _incomingOrders.Enqueue(order);
        }

        // Process orders asynchronously
        public async Task ProcessOrdersAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_incomingOrders.TryDequeue(out var order))
                {
                    // Ensure one thread accesses OrderBook at a time
                    await _semaphore.WaitAsync();
                    try
                    {
                        await _orderBook.AddOrderAsync(order);
                        var trades = await _matchingEngine.MatchAsync();

                        // Optional: broadcast trades or log
                        if (trades.Any())
                            Console.WriteLine($"Processed {trades.Count} trades for {order.Symbol}");
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                else
                {
                    await Task.Delay(1); // avoid CPU spin
                }
            }
        }
    }
}
