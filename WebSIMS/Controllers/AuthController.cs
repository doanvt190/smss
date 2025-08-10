using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSIMS.Controllers
{
    public class AuthController : Controller
    {
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
