using System.Threading.Tasks;
using JokeJunction.Domain.Entity;

namespace JokeJunction.DAL.Interfaces
{
    public interface IJokeRepository : IBaseRepository<Joke>
    {
        Task<Joke> GetByScore(int score);
    }
}