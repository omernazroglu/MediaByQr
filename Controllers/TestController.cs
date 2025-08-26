using Microsoft.AspNetCore.Mvc;

namespace MediaByQr.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index(string qr)
        {
            ViewBag.qr = qr;  
            return View();
        }
    }
}
