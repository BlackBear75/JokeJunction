using JokeJunction.Domain.Enum;
using JokeJunction.Domain.ViewModels.Joke;
using JokeJunction.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class CreativeController : Controller
{
    private readonly IJokeService _jokeService;

    public CreativeController(IJokeService jokeService)
    {
        _jokeService = jokeService;
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
            var response = await _jokeService.CreateJoke(jokeViewModel);

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