using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock_Trading_Simulation_API.Infrastrcuture;

namespace Stock_Trading_Simulation_API.Controllers
{
    [Route("orders/view")]
    [ApiController]
    public class OrdersViewController : ControllerBase
    {
        private readonly MatchingEngine _engine;

        public OrdersViewController(MatchingEngine engine)
        {
            _engine = engine;
        }

        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            // Returns every submitted order (processed or not)
            return Ok(_engine.AllOrders);
        }
    }
}