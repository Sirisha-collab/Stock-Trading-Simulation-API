using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;
using Stock_Trading_Simulation_API.Infrastrcuture;

namespace Stock_Trading_Simulation_API.Controllers
{
    [ApiController]
    [Route("api/stocks")]
    public class StocksController : ControllerBase
    {
        private readonly IOrderBook _orderBook;

        public StocksController(IOrderBook orderBook)
        {
            _orderBook = orderBook;
        }

        // GET: api/stocks
        [HttpGet]
        public IActionResult GetStocks()
        {
            var stocks = _orderBook.GetAllOrders()
                .GroupBy(o => o.Symbol)
                .Select(g => new
                {
                    Symbol = g.Key,
                    LastPrice = g.OrderByDescending(x => x.Timestamp)
                                 .First().Price,
                    TotalOrders = g.Count()
                });

            return Ok(stocks);
        }

        [HttpGet("{symbol}")]
        public IActionResult GetStock(string symbol)
        {
            var orders = _orderBook.GetAllOrders()
                .Where(o => o.Symbol.Equals(
                    symbol,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!orders.Any())
                return NotFound();

            var latest = orders
                .OrderByDescending(o => o.Timestamp)
                .First();

            return Ok(new
            {
                Symbol = latest.Symbol,
                CurrentPrice = latest.Price,
                TotalOrders = orders.Count
            });
        }

        [HttpGet("{symbol}/orderbook")]
        public IActionResult GetOrderBook(string symbol)
        {
            var orders = _orderBook.GetAllOrders()
                .Where(o => o.Symbol.Equals(
                    symbol,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!orders.Any())
                return NotFound();

            return Ok(new
            {
                Symbol = symbol.ToUpper(),

                BuyOrders = orders
                    .Where(o => o.Side == OrderSide.Buy)
                    .OrderByDescending(o => o.Price)
                    .ThenBy(o => o.Timestamp),

                SellOrders = orders
                    .Where(o => o.Side == OrderSide.Sell)
                    .OrderBy(o => o.Price)
                    .ThenBy(o => o.Timestamp)
            });
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var orders = _orderBook.GetAllOrders();

            var result = orders
                .GroupBy(o => o.Symbol)
                .Select(g =>
                {
                    var symbolOrders = g.ToList();

                    var buys = symbolOrders
                        .Where(o => o.Side == OrderSide.Buy && o.Status != OrderStatus.Cancelled)
                        .OrderByDescending(o => o.Price);

                    var sells = symbolOrders
                        .Where(o => o.Side == OrderSide.Sell && o.Status != OrderStatus.Cancelled)
                        .OrderBy(o => o.Price);

                    return new DashboardDTO
                    {
                        Symbol = g.Key,

                        LastPrice = null, // no real market price yet
                        LastTradeTime = null,

                        BestBid = buys.FirstOrDefault()?.Price,
                        BestAsk = sells.FirstOrDefault()?.Price,

                        ActiveOrderCount = symbolOrders.Count(o =>
                            o.Status == OrderStatus.Pending ||
                            o.Status == OrderStatus.Queued ||
                            o.Status == OrderStatus.PartiallyFilled)
                    };
                });

            return Ok(result);
        }

        [HttpGet("debug")]
        public IActionResult Debug()
        {
            var orders = _orderBook.GetAllOrders();

            return Ok(new
            {
                Count = orders.Count,
                Orders = orders
            });
        }
    }
}
