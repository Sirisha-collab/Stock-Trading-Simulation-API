using Microsoft.AspNetCore.Mvc;
using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;
using System.Reflection;

namespace Stock_Trading_Simulation_API.Controllers
{
    public class StocksUIController : Controller
    {
        private readonly IOrderBook _orderBook;

        public StocksUIController(IOrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        public IActionResult Index()
        {
            var orders = _orderBook.GetAllOrders();

            var model = orders
                .GroupBy(o => o.Symbol)
                .Select(g => new DashboardDTO
                {
                    Symbol = g.Key,
                    BestBid = g.Where(x => x.Side == OrderSide.Buy)
                               .OrderByDescending(x => x.Price)
                               .FirstOrDefault()?.Price,
                    ActiveOrderCount = g.Count()
                })
                .ToList();

            return View(model); // IMPORTANT
        }
    }
}
