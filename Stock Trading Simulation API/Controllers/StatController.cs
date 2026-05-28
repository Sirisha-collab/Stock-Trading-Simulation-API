using Microsoft.AspNetCore.Mvc;
using Stock_Trading_Simulation_API.Infrastrcuture;

namespace Stock_Trading_Simulation_API.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatController : Controller
    {
        private readonly MatchingEngine _engine;
        public StatController(MatchingEngine engine)
        {
            _engine = engine;
        }

        [HttpGet]
        public IActionResult Statistics()
        {
            return Ok(new
            {
                ordersProcessed = _engine.OrdersProcessed,
                tradesExecuted = _engine.TradesExecuted
            });
        }
    }
}
