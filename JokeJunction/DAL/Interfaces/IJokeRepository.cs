using System.Threading.Tasks;
using JokeJunction.Domain.Entity;

namespace JokeJunction.DAL.Interfaces
{
    public interface IJokeRepository : IBaseRepository<Joke>
    {
       

        Task<int> GetJokeVotes(Joke joke);

        Task<List<Joke>> GetUserJoke(ApplicationUser user);
        Task<bool> GetRatingUser(Joke joke, ApplicationUser currentUser);

        Task<bool> RemoveJokeRating(Joke joke, ApplicationUser user);

        Task<bool> HasUserRatedJoke(Joke joke, ApplicationUser user);


    }
}