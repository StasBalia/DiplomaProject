using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.DAL.Repositories.Records;
using SQLWorker.Web.Models.Request;

namespace SQLWorker.Web.Controllers
{
    public class AuthController : Controller
    {
        private UserService _userService;
        public AuthController(UserService userService)
        {
            _userService = userService;
        }
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

            UserDTO user = await _userService.AddAsync(model.Name, model.Password, model.Email);
            
            return await SignInUserAsync(user);
        }

        private async Task<IActionResult> SignInUserAsync(UserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }
    }
}