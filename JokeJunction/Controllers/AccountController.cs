using JokeJunction.Domain.Entity;
using JokeJunction.Domain.ViewModels.Account;
using JokeJunction.Domain.ViewModels.Joke;
using JokeJunction.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JokeJunction.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IJokeService _jokeService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
             IJokeService jokeService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jokeService = jokeService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }


		public async Task<IActionResult> Profile()
		{
			// Отримати ідентифікатор поточного користувача
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


			// Отримати користувача з бази даних за ідентифікатором
			var user = await _userManager.FindByIdAsync(userId);

            var response = await _jokeService.GetUserJoke(user);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {

                foreach (var item in response.Data)
                {
                    user.Jokes.Add(item);
                }
            }
                return View(user);
            
           
		}

        [HttpPost]
        public async Task<IActionResult> DeleteJoke(int jokeId)
        {
            var response = await _jokeService.DeleteJoke(jokeId);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(response.Data); // Відповідь зі статусом 200 і значенням, якщо ви вирішили повертати значення з функції видалення
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.Description); // Відповідь з відповідним статусом помилки і описом помилки
            }
        }



    }
}
