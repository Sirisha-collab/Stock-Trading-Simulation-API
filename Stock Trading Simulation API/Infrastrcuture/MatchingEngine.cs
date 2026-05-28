using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;

namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class MatchingEngine : IMatchingEngine
    {
        private readonly IOrderBook _orderBook;
        private readonly OrderQueue _queue;

        public long OrdersProcessed;
        public long TradesExecuted;

        public MatchingEngine(IOrderBook orderBook, OrderQueue queue)
        {
            _orderBook = orderBook;
            _queue = queue;
        }
        public List<Order> AllOrders = new List<Order>();
        public async Task Run(CancellationToken token)
        {
            await foreach (var order in _queue.ReadAll(token))
            {
                Console.WriteLine($"Processing order: {order.Id}, {order.Symbol}, {order.Price}, {order.Quantity}, {order.Side}");
                AllOrders.Add(order); // keep every order
                Console.WriteLine($"Processing order {order.Id}");
                await _orderBook.AddOrderAsync(order);

                var trades = await MatchAsync();

                OrdersProcessed++;

                TradesExecuted += trades.Count;
            }
        }

        public async Task GenerateOrders(OrderQueue queue)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 1_000_000; i++)
            {
                var order = new Order
                {
                    Symbol = "AAPL",
                    Price = Random.Shared.Next(100, 200),
                    Quantity = 10,
                    Side = i % 2 == 0 ? OrderSide.Buy : OrderSide.Sell
                };

                tasks.Add(queue.Enqueue(order).AsTask());
            }

            await Task.WhenAll(tasks);
        }

        public async Task<List<Trade>> MatchAsync()
        {
            var trades = new List<Trade>();

            var book = (OrderBook)_orderBook;
            await book.Lock.WaitAsync();

            try
            {
                while (_orderBook.HasMatch())
                {
                    var buy = _orderBook.GetBestBuy();
                    var sell = _orderBook.GetBestSell();

                    var quantity = Math.Min(buy.Quantity, sell.Quantity);

                    trades.Add(new Trade
                    {
                        BuyOrderId = buy.Id,
                        SellOrderId = sell.Id,
                        Price = sell.Price,
                        Quantity = quantity
                    });

                    buy.Quantity -= quantity;
                    sell.Quantity -= quantity;

                    _orderBook.RemoveBestBuy();
                    if (sell.Quantity == 0)
                    {
                        _orderBook.RemoveBestSell();  //added for Ultra-Fast Order Book (Price-Time Priority)
                    }
                }
            }
            finally
            {
                book.Lock.Release();
            }

            return trades;
        }
    }
}
