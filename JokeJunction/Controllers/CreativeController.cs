using JokeJunction.Domain.Entity;
using JokeJunction.Domain.Enum;
using JokeJunction.Domain.ViewModels.Joke;
using JokeJunction.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class CreativeController : Controller
{
    private readonly IJokeService _jokeService;
    private readonly UserManager<ApplicationUser> _userManager;
    public CreativeController(IJokeService jokeService, UserManager<ApplicationUser> userManager)
    {
        _jokeService = jokeService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Creative()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateJoke(JokeViewModel jokeViewModel)
    {
        if (ModelState.IsValid)
        {

            var user = await _userManager.GetUserAsync(User);
            var response = await _jokeService.CreateJoke(jokeViewModel, user);



            if (response.StatusCode == JokeJunction.Domain.Enum.StatusCode.OK)
            {
                // Успішно створено жарт, можна виконати додаткові дії або повернутися на головну сторінку
                return RedirectToAction("Index", "Home");
            }

            // Якщо щось пішло не так, можна обробити помилку або повернути користувача назад на форму
            ModelState.AddModelError(string.Empty, response.Description);
        }

        // Повернення на сторінку з формою з помилками, якщо дані невірні
        return View("Creative", jokeViewModel);
    }
}