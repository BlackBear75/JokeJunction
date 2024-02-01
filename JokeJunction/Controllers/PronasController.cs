using Microsoft.AspNetCore.Mvc;

namespace JokeJunction.Controllers
{
    public class PronasController : Controller
    {
        public IActionResult Pronas()
        {
            return View();
        }
    }
}
