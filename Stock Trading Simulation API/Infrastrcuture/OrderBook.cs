using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;

namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class OrderBook : IOrderBook
    {
        private readonly SortedDictionary<decimal, Queue<Order>> _buyOrders =
        new(Comparer<decimal>.Create((x, y) => y.CompareTo(x))); // Descending

        private readonly SortedDictionary<decimal, Queue<Order>> _sellOrders =
            new(); // Ascending

        private readonly SemaphoreSlim _lock = new(1, 1);

        public async Task AddOrderAsync(Order order)
        {
            await _lock.WaitAsync();

            try
            {
                order.Status = OrderStatus.Queued;

                var book = order.Side == OrderSide.Buy
                    ? _buyOrders
                    : _sellOrders;

                if (!book.ContainsKey(order.Price))
                    book[order.Price] = new Queue<Order>();

                book[order.Price].Enqueue(order);
            }
            finally
            {
                _lock.Release();
            }
        }

        public Order? GetOrder(Guid orderId)
        {
            foreach (var level in _buyOrders.Values)
            {
                var order = level.FirstOrDefault(x => x.Id == orderId);

                if (order != null)
                    return order;
            }

            foreach (var level in _sellOrders.Values)
            {
                var order = level.FirstOrDefault(x => x.Id == orderId);

                if (order != null)
                    return order;
            }

            return null;
        }

        public bool HasMatch()
        {
            if (!_buyOrders.Any() || !_sellOrders.Any())
                return false;

            var bestBuyPrice = _buyOrders.First().Key;
            var bestSellPrice = _sellOrders.First().Key;

            return bestBuyPrice >= bestSellPrice;
        }

        public Order GetBestBuy()
            => _buyOrders.First().Value.Peek();

        public Order GetBestSell()
            => _sellOrders.First().Value.Peek();

        public Order PeekBestBuy()
        {
            return _buyOrders.First().Value.Peek();
        }

        public Order PeekBestSell()
        {
            return _sellOrders.First().Value.Peek();
        }

        public void RemoveBestBuy()
        {
            var best = _buyOrders.First();
            best.Value.Dequeue();

            if (best.Value.Count == 0)
                _buyOrders.Remove(best.Key);
        }

        public void RemoveBestSell()
        {
            var best = _sellOrders.First();
            best.Value.Dequeue();

            if (best.Value.Count == 0)
                _sellOrders.Remove(best.Key);
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            foreach (var level in _buyOrders)
                orders.AddRange(level.Value.Where(o => o.Quantity > 0));

            foreach (var level in _sellOrders)
                orders.AddRange(level.Value.Where(o => o.Quantity > 0));

            return orders;
        }
        public SemaphoreSlim Lock => _lock; // expose lock for engine
    }
}
