using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SQLWorker.Web.Models.Request;

namespace SQLWorker.Web.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await Task.Run(() => new EmptyResult());
        }
    }
}