using JokeJunction.Domain.Entity;
using JokeJunction.Domain.Response;
using JokeJunction.Domain.ViewModels.Joke;

namespace JokeJunction.Servise.Interfaces
{
    public interface IUserServise
    {
        Task<IBaseResponse<IEnumerable<ApplicationUser>>> GetUsers();

        Task<IBaseResponse<ApplicationUser>> GetUserById(int id);

        Task<IBaseResponse<ApplicationUser>> CreateUser(ApplicationUser carViewModel);

        Task<IBaseResponse<bool>> DeleteUser(int id);

        Task<IBaseResponse<ApplicationUser>> Edit(int id, ApplicationUser model);
    }
}
