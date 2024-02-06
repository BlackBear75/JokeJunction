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

        public async Task<IBaseResponse<JokeViewModel>> GetJoke(int id)
        {
            var baseResponse = new BaseResponse<JokeViewModel>();
            try
            {
                var joke = await _jokeRepository.Get(id);
                if (joke == null)
                {
                    baseResponse.Description = "User not found";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    return baseResponse;
                }

                var data = new JokeViewModel()
                {
                    TypeJoke = joke.TypeJoke.ToString(),
                    Content = joke.Content,
                    Name = joke.Name,

                };

                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = data;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<JokeViewModel>()
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

        public async Task<IBaseResponse<Joke>> GetJokeByScore(int score)
        {
            var baseResponse = new BaseResponse<Joke>();
            try
            {
                var car = await _jokeRepository.GetByScore(score);
                if (car == null)
                {
                    baseResponse.Description = "User not found";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    return baseResponse;
                }

                baseResponse.Data = car;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Joke>()
                {
                    Description = $"[GetCarByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Joke>> Edit(int id, JokeViewModel model)
        {
            var baseResponse = new BaseResponse<Joke>();
            try
            {
                var joke = await _jokeRepository.Get(id);
                if (joke == null)
                {
                    baseResponse.StatusCode = StatusCode.CarNotFound;
                    baseResponse.Description = "Car not found";
                    return baseResponse;
                }

                joke.Content = model.Content;
                joke.Name = model.Name;
               
               

                await _jokeRepository.Update(joke);


                return baseResponse;
                // TypeCar

            }
            catch (Exception ex)
            {
                return new BaseResponse<Joke>()
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
                    joke.Id = number;
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
    }   
}