using JokeJunction.DAL.Interfaces;
using JokeJunction.DAL.Repositories;
using JokeJunction.Domain.Entity;
using JokeJunction.Domain.Enum;
using JokeJunction.Domain.Response;
using JokeJunction.Domain.ViewModels.Joke;
using JokeJunction.Servise.Interfaces;

namespace JokeJunction.Servise.Implementations
{
    public class UserServise : IUserServise
    {
        private readonly IUserRepository _userRepository;

        public UserServise(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<IBaseResponse<ApplicationUser>> CreateUser(ApplicationUser userViewModel)
        {
            var baseResponse = new BaseResponse<ApplicationUser>();
            try
            {
                var existingUserByName = await _userRepository.GetUserByName(userViewModel.UserName);
                var existingUserByEmail = await _userRepository.GetUserByEmail(userViewModel.Email);

                if (existingUserByName != null || existingUserByEmail != null)
                {
                    return new BaseResponse<ApplicationUser>()
                    {
                        Description = "Користувач з таким ім'ям або електронною адресою вже існує.",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                var user = new ApplicationUser()
                {
                    PasswordHash = userViewModel.PasswordHash,
                    Email = userViewModel.Email,
                    UserName = userViewModel.UserName,
                };

               
                await _userRepository.Create(user);
            }
            catch (Exception ex)
            {
                // Логування помилок може бути додано сюди
                return new BaseResponse<ApplicationUser>()
                {
                    Description = $"[CreateUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            return baseResponse;
        }

        public async Task<IBaseResponse<bool>> DeleteUser(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var userToDelete = await _userRepository.Get(id);
                if (userToDelete != null)
                {
                    _userRepository.Delete(userToDelete);
                    baseResponse.Data = true; // Користувач видалений
                }
                else
                {
                    baseResponse.Description = "Користувач не знайдений.";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    baseResponse.Data = false;
                }
            }
            catch (Exception ex)
            {
                baseResponse.Description = $"[DeleteUser] : {ex.Message}";
                baseResponse.StatusCode = StatusCode.InternalServerError;
                baseResponse.Data = false;
            }

            return baseResponse;
        }

        public async Task<IBaseResponse<ApplicationUser>> Edit(int id, ApplicationUser model)
        {
            var baseResponse = new BaseResponse<ApplicationUser>();
            try
            {
                var existingUser = await _userRepository.Get(id);
                if (existingUser != null)
                {
                    existingUser.UserName = model.UserName;
                    existingUser.Email = model.Email;
                    existingUser.PasswordHash = model.PasswordHash;

                    baseResponse.Data = existingUser; // Користувач відредагований
                }
                else
                {
                    baseResponse.Description = "Користувач не знайдений.";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    
                }
            }
            catch (Exception ex)
            {
                baseResponse.Description = $"[EditUser] : {ex.Message}";
                baseResponse.StatusCode = StatusCode.InternalServerError;
             
            }

            return baseResponse;
        }

        public async Task<IBaseResponse<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var baseResponse = new BaseResponse<IEnumerable<ApplicationUser>>();
            try
            {
                var jokes = await _userRepository.Select();
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
                baseResponse.Description = $"[GetUsers] : {ex.Message}";
                baseResponse.StatusCode = StatusCode.InternalServerError;
            }

            return baseResponse;
        }
        public async Task<IBaseResponse<ApplicationUser>> GetUserById(int id)
        {
            var baseResponse = new BaseResponse<ApplicationUser>();
            try
            {
                baseResponse.Data = await _userRepository.Get(id);
                if (baseResponse.Data == null)
                {
                    baseResponse.Description = "Користувач не знайдений.";
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                baseResponse.Description = $"[GetUserById] : {ex.Message}";
                baseResponse.StatusCode = StatusCode.InternalServerError;
            }

            return baseResponse;
        }
    }
}
