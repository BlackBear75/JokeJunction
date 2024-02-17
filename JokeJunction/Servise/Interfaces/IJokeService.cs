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
        
        Task<IBaseResponse<Joke>> GetJoke(int id);


 

        Task<IBaseResponse<IEnumerable<Joke>>> GetUserJoke(ApplicationUser user);

           
        Task<IBaseResponse<Joke>> CreateJoke(JokeViewModel carViewModel,ApplicationUser user);

        Task<IBaseResponse<bool>> DeleteJoke(int id);
        Task<IBaseResponse<int>> GetJokeVotes(int id);
       Task<IBaseResponse<Joke>> AddJokeRating(int id, float rating, ApplicationUser user);

        Task<IBaseResponse<bool>> RemoveJokeRating(int id, ApplicationUser user);

        Task<IBaseResponse<JokeViewModel>> Edit(int id, JokeViewModel model);
        Task<IBaseResponse<bool>> CheckUserRating(int id, ApplicationUser user);
        
    }   
}