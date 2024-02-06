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

        Task<IBaseResponse<IEnumerable<Joke>>> GetUserJoke(ApplicationUser user);


        Task<IBaseResponse<Joke>> CreateJoke(JokeViewModel carViewModel,ApplicationUser user);

        Task<IBaseResponse<bool>> DeleteJoke(int id);

        Task<IBaseResponse<Joke>> GetJokeByScore(int score);

        Task<IBaseResponse<Joke>> Edit(int id, JokeViewModel model);
    }   
}