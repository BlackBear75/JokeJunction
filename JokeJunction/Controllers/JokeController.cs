using System.Linq;
using System.Threading.Tasks;
using JokeJunction.Domain.Entity;
using JokeJunction.Domain.ViewModels.Joke;
using JokeJunction.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JokeJunction.Controllers
{
    public class JokeController : Controller
    {
        private const int PageSize = 6;

        private readonly IJokeService _carService;

        public JokeController(IJokeService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJokes(int page = 1)
        {
            var response = await _carService.GetJokes();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var totalCount = response.Data.Count();
                var jokes = response.Data.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var model = new JokesViewModel
                {
                    Jokes = jokes,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    }
                };

                return View(model);
            }
            else
            {
                // Обробка випадку, коли відповідь не є OK
                return RedirectToAction("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Black(int page = 1)
        {
            var response = await _carService.GetJokes();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                // Сортування за полем TypeJoke зі значенням 4
                var jokes = response.Data
                    .Where(j => (int)j.TypeJoke == 3)
                    .OrderBy(j => j.TypeJoke)
                    .ToList();

                var totalCount = jokes.Count();
                jokes = jokes.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var model = new JokesViewModel
                {
                    Jokes = jokes,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    }
                };

                return View(model);
            }
            else
            {
                // Обробка випадку, коли відповідь не є OK
                return RedirectToAction("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Criminal(int page = 1)
        {
            var response = await _carService.GetJokes();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                // Сортування за полем TypeJoke зі значенням 4
                var jokes = response.Data
                    .Where(j => (int)j.TypeJoke == 1)
                    .OrderBy(j => j.TypeJoke)
                    .ToList();

                var totalCount = jokes.Count();
                jokes = jokes.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var model = new JokesViewModel
                {
                    Jokes = jokes,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    }
                };

                return View(model);
            }
            else
            {
                // Обробка випадку, коли відповідь не є OK
                return RedirectToAction("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Humorous(int page = 1)
        {
            var response = await _carService.GetJokes();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                // Сортування за полем TypeJoke зі значенням 4
                var jokes = response.Data
                    .Where(j => (int)j.TypeJoke == 0)
                    .OrderBy(j => j.TypeJoke)
                    .ToList();

                var totalCount = jokes.Count();
                jokes = jokes.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var model = new JokesViewModel
                {
                    Jokes = jokes,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    }
                };

                return View(model);
            }
            else
            {
                // Обробка випадку, коли відповідь не є OK
                return RedirectToAction("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> IT_Joke(int page = 1)
        {
            var response = await _carService.GetJokes();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                // Сортування за полем TypeJoke зі значенням 4
                var jokes = response.Data
                    .Where(j => (int)j.TypeJoke == 4)
                    .OrderBy(j => j.TypeJoke)
                    .ToList();

                var totalCount = jokes.Count();
                jokes = jokes.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var model = new JokesViewModel
                {
                    Jokes = jokes,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    }
                };

                return View(model);
            }
            else
            {
                // Обробка випадку, коли відповідь не є OK
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Politic(int page = 1)
        {
            var response = await _carService.GetJokes();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                // Сортування за полем TypeJoke зі значенням 4
                var jokes = response.Data
                    .Where(j => (int)j.TypeJoke == 2)
                    .OrderBy(j => j.TypeJoke)
                    .ToList();

                var totalCount = jokes.Count();
                jokes = jokes.Skip((page - 1) * PageSize).Take(PageSize).ToList();

                var model = new JokesViewModel
                {
                    Jokes = jokes,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    }
                };

                return View(model);
            }
            else
            {
                // Обробка випадку, коли відповідь не є OK
                return RedirectToAction("Error");
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetJoke(int id)
        {
            var response = await _carService.GetJoke(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _carService.DeleteJoke(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetJokes");
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        /*[Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0)
            {
                return View();
            }

            var response = await _carService.GetJoke(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            
            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Save(JokeViewModel model,ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    await _carService.CreateJoke(model,user);
                }
                else
                {
                    await _carService.Edit(model.Id, model);
                }
            }

            return RedirectToAction("GetJokes");
        }
    }
}