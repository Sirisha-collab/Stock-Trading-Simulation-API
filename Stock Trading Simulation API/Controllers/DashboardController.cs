using Microsoft.AspNetCore.Mvc;

namespace Stock_Trading_Simulation_API.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
