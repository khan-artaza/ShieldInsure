using Final_Insure.Models;

namespace Final_Insure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
    }
}