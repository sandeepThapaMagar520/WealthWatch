using WealthWatch.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WealthWatch.Services
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsersAsync();
        Task<Users?> GetUserByIdAsync(Guid id);
        Task<string> GetNameByIdAsync(Guid id);
        Task<Users?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(Users user);
    }
}
