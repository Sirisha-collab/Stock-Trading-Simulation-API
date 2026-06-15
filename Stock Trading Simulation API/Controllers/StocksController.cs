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
    }
}
