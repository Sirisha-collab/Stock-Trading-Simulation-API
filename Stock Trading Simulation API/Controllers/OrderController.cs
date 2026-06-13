using Microsoft.AspNetCore.Mvc;
using Stock_Trading_Simulation_API.Application;
using Stock_Trading_Simulation_API.Domain_Model;
using Stock_Trading_Simulation_API.Infrastrcuture;
using System.Diagnostics;

namespace Stock_Trading_Simulation_API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderBook _orderBook;
        private readonly IMatchingEngine _engine;
        private readonly OrderProcessor _processor;

        public OrderController(IOrderBook orderBook, IMatchingEngine engine, OrderProcessor processor)
        {
            _orderBook = orderBook;
            _engine = engine;
            _processor = processor; 
        }

        [HttpPost("placeorder")]
        public IActionResult PlaceOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Invalid order payload");

            _processor.EnqueueOrder(order);

            Console.WriteLine($"Order queued {order.Id}");

            return Accepted(new
            {
                Message = "Order received and queued",
                OrderId = order.Id
            });
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("Stock Trading API Running!!!");
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var book = (OrderBook)_orderBook;

            return Ok(book.GetAllOrders());
        }

        [HttpGet("status/{orderId}")]
        public IActionResult GetOrderStatus(Guid orderId)
        {
            var book = (OrderBook)_orderBook;

            var order = book.GetOrder(orderId);

            if (order == null)
                return NotFound("Order not found");

            return Ok(new
            {
                order.Id,
                order.Symbol,
                order.Side,
                order.Price,
                order.Quantity,
                order.Status,
                order.Timestamp
            });
        }
    }
}
