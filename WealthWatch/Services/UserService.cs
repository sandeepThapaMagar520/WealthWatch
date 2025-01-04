using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WealthWatch.Models;

namespace WealthWatch.Services
{
    public class UserService
    {
        private readonly string _dataFilePath;

        public UserService()
        {
            // Set the file path to the desired directory
            string appDataPath = Path.Combine("D:\\final year\\application development");
            _dataFilePath = Path.Combine(appDataPath, "users.json");

            // Ensure the directory exists
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
        }

        private async Task SaveUserAsync(List<Users> users)
        {
            string json = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_dataFilePath, json);
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    return new List<Users>();
                }

                string jsonUsers = await File.ReadAllTextAsync(_dataFilePath);
                return JsonSerializer.Deserialize<List<Users>>(jsonUsers) ?? new List<Users>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing users.json: {ex.Message}");
                return new List<Users>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Users>();
            }
        }

        public async Task<Users?> GetUserByIdAsync(Guid id)
        {
            var users = await GetAllUsersAsync();
            return users.FirstOrDefault(x => x.Id == id);
        }
        public async Task<string> GetNameByIdAsync(Guid id)
        {
            var users = await GetAllUsersAsync();
            Users foundUser =users.FirstOrDefault(x => x.Id == id);
            return foundUser.FullName;
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            var users = await GetAllUsersAsync();
            return users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> CreateUserAsync(Users user)
        {
            var users = await GetAllUsersAsync();

            if (users.Any(u => u.Email == user.Email))
            {
                return 2; // User already exists
            }

            users.Add(user);
            await SaveUserAsync(users);
            return 1;
        }
    }
}