using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;


namespace KairoAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class MainController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MainController> _logger;
        public MainController(ILogger<MainController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
