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

        private readonly OrderQueue _queue;

        public OrderController(IOrderBook orderBook, IMatchingEngine engine, OrderProcessor processor, OrderQueue queue)
        {
            _orderBook = orderBook;
            _engine = engine;
            _processor = processor; 
            _queue = queue;
        }
        [HttpPost("placeorder")]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            //await _orderBook.AddOrderAsync(order);
            //var trades = await _engine.MatchAsync();
            //return Ok(trades);
            await _queue.Enqueue(order);
            Console.WriteLine($"Order queued {order.Id}");
            return Ok("Order received and queued");

        }
        [HttpPost("replaceorder")]
        public IActionResult ReplaceOrder(Order order)
        {
            _processor.EnqueueOrder(order); // enqueue order
            return Accepted("Order enqueued and will be processed asynchronously");
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
