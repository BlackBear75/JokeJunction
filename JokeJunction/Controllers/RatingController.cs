using JokeJunction.Domain.Entity;
using JokeJunction.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JokeJunction.Controllers
{
    public class RatingController : Controller
    {

         private readonly IJokeService _jokeService;
        private readonly UserManager<ApplicationUser> _userManager;
        public RatingController(IJokeService jokeService, UserManager<ApplicationUser> userManager)
        {
            _jokeService = jokeService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetJokeVotes(int jokeId)
        {
            try
            {
                // Отримати кількість голосів для жарту з використанням сервісу
                var jokeVotes = await _jokeService.GetJokeVotes(jokeId);
                return Ok(jokeVotes.Data);
            }
            catch (Exception ex)
            {
                // Обробка помилки у випадку виникнення винятку
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


      

        [HttpPost]
        public async Task<IActionResult> RateJoke(int jokeId, float rating)
        {
            // Отримуємо поточного користувача
            var currentUser = await _userManager.GetUserAsync(User);

            // Викликаємо метод сервісу для додавання оцінки до жарту
            var response = await _jokeService.AddJokeRating(jokeId, rating, currentUser);

            // Перевіряємо статус відповіді
            if (response.StatusCode ==Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("Index", "Home"); // Редірект на головну сторінку або на сторінку з жартами
            }
            else
            {
                // Якщо сталася помилка, повертаємо перегляд з повідомленням про помилку
                ModelState.AddModelError("", response.Description);
                return View(); // Передумовоно, що у вас є відповідний перегляд для обробки оцінювання жарту
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveJokeRating(int jokeId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account"); // або інше редирект для авторизації
            }

            var response = await _jokeService.RemoveJokeRating(jokeId,currentUser);

            // Перевіряємо статус відповіді
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("Index", "Home"); // Редірект на головну сторінку або на сторінку з жартами
            }
            else
            {
                // Якщо сталася помилка, повертаємо перегляд з повідомленням про помилку
                ModelState.AddModelError("", response.Description);
                return View(); // Передумовоно, що у вас є відповідний перегляд для обробки оцінювання жарту
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckUserRating(int jokeId)
        {
            var currentUser = await _userManager.GetUserAsync(User); // Приклад отримання поточного користувача, можливо, вам потрібно відредагувати цю логіку

            if (currentUser == null)
            {
                return NotFound(); // Або будь-який інший відповідний код статусу
            }

            var userRating = await _jokeService.CheckUserRating(jokeId, currentUser); 

            if (userRating != null)
            {
                return Ok(userRating.Data); // Повернення інформації про оцінку користувача, наприклад, вже існуючий рейтинг або інші дані
            }
            else
            {
                return NotFound(); // Або будь-який інший відповідний код статусу, якщо користувач не оцінив жарт
            }
        }


    }
}
