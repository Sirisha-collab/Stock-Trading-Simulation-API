using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;

namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class MatchingEngine : IMatchingEngine
    {
        private readonly IOrderBook _orderBook;

        public long OrdersProcessed;
        public long TradesExecuted;

        public MatchingEngine(IOrderBook orderBook)
        {
            _orderBook = orderBook;
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

                    if (buy.Quantity == 0)
                        _orderBook.RemoveBestBuy();

                    if (sell.Quantity == 0)
                        _orderBook.RemoveBestSell();
                }

                OrdersProcessed++;
                TradesExecuted += trades.Count;
            }
            finally
            {
                book.Lock.Release();
            }

            return trades;
        }
    }
}