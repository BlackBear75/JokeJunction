using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JokeJunction.DAL.Interfaces;
using JokeJunction.Domain.Entity;
using JokeJunction.Domain.Enum;
using JokeJunction.Domain.Response;
using JokeJunction.Domain.ViewModels.Joke;
using JokeJunction.Service.Interfaces;

namespace JokeJunction.Service.Implementations
{
    public class JokeService : IJokeService
    {
        private readonly IJokeRepository _jokeRepository;

        public JokeService(IJokeRepository carRepository)
        {
            _jokeRepository = carRepository;
        }

        public async Task<IBaseResponse<Joke>> GetJoke(int id)
        {
            var baseResponse = new BaseResponse<Joke>();
            try
            {
                var joke = await _jokeRepository.Get(id);
                if (joke == null)
                {
                    baseResponse.Description = "User not found";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    return baseResponse;
                }

                var data = new Joke()
                {
                    TypeJoke = joke.TypeJoke,
                    Content = joke.Content,
                    Name = joke.Name,

                };

                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = data;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Joke>()
                {
                    Description = $"[GetCar] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Joke>> CreateJoke(JokeViewModel jokeViewModel, ApplicationUser user)
        {
            var baseResponse = new BaseResponse<Joke>();
            try
            {
                var joke = new Joke()
                {
                    
                    UserId = user.Id,
                    Content = jokeViewModel.Content,
                    Name = jokeViewModel.Name,
                    TypeJoke = Enum.Parse<TypeJoke>(jokeViewModel.TypeJoke)
                };

                await _jokeRepository.Create(joke);


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = joke;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Joke>()
                {
                    Description = $"[CreateJoke] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<bool>> DeleteJoke(int id)
        {
            var baseResponse = new BaseResponse<bool>()
            {
                Data = true
            };
            try
            {
                var car = await _jokeRepository.Get(id);
                if (car == null)
                {
                    baseResponse.Description = "User not found";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    baseResponse.Data = false;
                    
                    return baseResponse;
                }

                await _jokeRepository.Delete(car);

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteCar] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

       

        public async Task<IBaseResponse<JokeViewModel>> Edit(int id, JokeViewModel model)
        {
            var baseResponse = new BaseResponse<JokeViewModel>();
            try
            {
                var joke = await _jokeRepository.Get(id);
                if (joke == null)
                {
                    baseResponse.StatusCode = StatusCode.JokeNotFound;
                    baseResponse.Description = "Car not found";
                    return baseResponse;
                }

                joke.Content = model.Content;
                joke.Name = model.Name;
               
               
               

                await _jokeRepository.Update(joke);


                return baseResponse;
              

            }
            catch (Exception ex)
            {
                return new BaseResponse<JokeViewModel>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Joke>>> GetJokes()
        {
            var baseResponse = new BaseResponse<IEnumerable<Joke>>();
            try
            {
                var jokes = await _jokeRepository.Select();
                if (jokes.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    return baseResponse;
                }
                int number = 1;
               foreach (var joke in jokes)
                {
                    joke.Number = number;
                    number++;
                }
                
                baseResponse.Data = jokes;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Joke>>()
                {
                    Description = $"[GetCars] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<Joke>>> GetUserJoke(ApplicationUser user)
        {
            var baseResponse = new BaseResponse<IEnumerable<Joke>>();
            try
            {
                var jokes = await _jokeRepository.GetUserJoke(user);

                if (jokes.Count == 0)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    return baseResponse;
                }

                baseResponse.Data = jokes;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Joke>>()
                {
                    Description = $"[GetJokes] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }



        public async Task<IBaseResponse<Joke>> AddJokeRating(int id, float rating, ApplicationUser user)
        {
            var baseResponse = new BaseResponse<Joke>();
            try
            {
                var joke = await _jokeRepository.Get(id);

                if (joke == null)
                {
                    baseResponse.Description = "Жарт не знайдено";
                    baseResponse.StatusCode = StatusCode.JokeNotFound;
                    return baseResponse;
                }

                // Перевірка, чи користувач вже оцінив цей жарт
                var userRating = await _jokeRepository.GetRatingUser(joke, user);

                if (userRating)
                {
                    baseResponse.Description = "Ви вже оцінили цей жарт";
                    baseResponse.StatusCode = StatusCode.AlreadyRated;
                    return baseResponse;
                }

                var newRating = new Rating { Value = rating, UserId = user.Id };

                joke.Ratings.Add(newRating);

                joke = await _jokeRepository.Update(joke);
                baseResponse.Data = joke;
                baseResponse.StatusCode = StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Joke>()
                {
                    Description = $"Помилка при додаванні оцінки до жарту: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<int>> GetJokeVotes(int id)
        {
            var baseResponse = new BaseResponse<int>();
            try
            {
                var joke = await _jokeRepository.Get(id);

                if (joke == null)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.JokeNotFound;
                    return baseResponse;
                }
                
                baseResponse.Data = await _jokeRepository.GetJokeVotes(joke);
                baseResponse.StatusCode= StatusCode.OK;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    Description = $"[GetJokesVotes] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> RemoveJokeRating(int id, ApplicationUser user)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var joke = await _jokeRepository.Get(id);

                if (joke == null)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.JokeNotFound;
                    return baseResponse;
                }

                baseResponse.Data = await _jokeRepository.RemoveJokeRating(joke, user);

                if(baseResponse.Data)
                {
                    baseResponse.Description = "Жарт був успішно видалений";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }
                else
                {
                    baseResponse.Description = "Жарт не був видаленний";
                    baseResponse.StatusCode = StatusCode.NotRemove;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[RemoveJokeRating] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> CheckUserRating(int id, ApplicationUser user)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var joke = await _jokeRepository.Get(id);

                if (joke == null)
                {
                    baseResponse.Description = "Найдено 0 элементов";
                    baseResponse.StatusCode = StatusCode.JokeNotFound;
                    return baseResponse;
                }

                baseResponse.Data = await _jokeRepository.HasUserRatedJoke(joke, user);

                if (baseResponse.Data)
                {
                    baseResponse.Description = "Жарт був оцінений";
                    baseResponse.StatusCode = StatusCode.JokeRated;
                    return baseResponse;
                }
                else
                {
                    baseResponse.Description = "Жарт не був неоцінений";
                    baseResponse.StatusCode = StatusCode.NotJokeRated;
                    return baseResponse;
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[RemoveJokeRating] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}