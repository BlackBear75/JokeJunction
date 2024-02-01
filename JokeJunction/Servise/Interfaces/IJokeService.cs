using System.Collections.Generic;
using System.Threading.Tasks;
using JokeJunction.Domain.Entity;
using JokeJunction.Domain.Response;
using JokeJunction.Domain.ViewModels.Joke;

namespace JokeJunction.Service.Interfaces
{
    public interface IJokeService
    {
        Task<IBaseResponse<IEnumerable<Joke>>> GetJokes();
        
        Task<IBaseResponse<JokeViewModel>> GetJoke(int id);

        Task<IBaseResponse<JokeViewModel>> CreateJoke(JokeViewModel carViewModel);

        Task<IBaseResponse<bool>> DeleteJoke(int id);

        Task<IBaseResponse<Joke>> GetJokeByScore(int score);

        Task<IBaseResponse<Joke>> Edit(int id, JokeViewModel model);
    }   
}