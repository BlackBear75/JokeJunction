using JokeJunction.Domain.Entity;

namespace JokeJunction.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUserByName(string userName);
        Task<ApplicationUser> GetUserByEmail(string email);


    }
}

