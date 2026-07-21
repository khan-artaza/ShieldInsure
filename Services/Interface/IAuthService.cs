using Final_Insure.DTOs;
using Final_Insure.Models;

namespace Final_Insure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(UserLoginDTO loginDto);
        Task<User> RegisterAsync(UserRegisterDTO registerDto);
    }
}