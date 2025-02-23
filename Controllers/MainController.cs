using KairoAPI.Impl;
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
            ClassAccess.MainController = this;
        }
        public IActionResult Index()
        {
            Log(Request, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            return Program.VULJsonResult;
        }
        private static void Log(HttpRequest request, string ip)
        {
            Console.WriteLine($"{request.Host} {request.Path} {request.Protocol} {request.Headers.UserAgent}");
            ClassAccess.MainController._logger.LogInformation($"{DateTime.Now} {DateTime.Now - DateTime.UtcNow} {ip} {request.Host} {request.Path} {request.Protocol} {request.Headers.UserAgent}");
        }
        private static void Log(Exception ex)
        {
            ClassAccess.MainController._logger.LogError(ex.Message);
            ClassAccess.MainController._logger.LogError(ex.StackTrace);
            if (ex.InnerException != null)
            {
                ClassAccess.MainController._logger.LogError(ex.InnerException.Message);
                ClassAccess.MainController._logger.LogError(ex.InnerException.StackTrace);
            }
        }
    }

}
