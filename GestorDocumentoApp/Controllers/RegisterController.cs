using Microsoft.AspNetCore.Mvc;

namespace GestorDocumentoApp.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
