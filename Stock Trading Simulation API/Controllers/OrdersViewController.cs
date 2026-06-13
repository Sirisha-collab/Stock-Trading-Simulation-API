using Microsoft.AspNetCore.Mvc;
using Stock_Trading_Simulation_API.Application;

namespace Stock_Trading_Simulation_API.Controllers
{
    [Route("orders/view")]
    [ApiController]
    public class OrdersViewController : ControllerBase
    {
        private readonly IOrderBook _orderBook;

        public OrdersViewController(IOrderBook orderBook)
        {
            _orderBook = orderBook;
        }

        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            return Ok(_orderBook.GetAllOrders());
        }
    }
}