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
