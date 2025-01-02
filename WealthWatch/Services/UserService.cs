using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WealthWatch");
            _dataFilePath = Path.Combine(appDataPath, "users.json");
            Console.WriteLine(_dataFilePath);

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
        }
        private void SaveUser(List<Users> user)
        {
            string json = JsonSerializer.Serialize(user);
            File.WriteAllText(_dataFilePath, json);
        }

        public List<Users> GetAllUsers()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    Console.WriteLine("json file not found!");
                    return new List<Users>();
                }

                string jsonUsers = File.ReadAllText(_dataFilePath);
                var users = JsonSerializer.Deserialize<List<Users>>(jsonUsers) ?? new List<Users>();
                return users;
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

        public Users? GetUserById(Guid id)
        {
            return GetAllUsers().FirstOrDefault(x => x.Id == id);
        }

        public Users? GetUserByEmail(String Email)
        {
            var users = GetAllUsers();
            if(users == null)
            {
                Console.WriteLine("no users on the file!");
            }
            var user = users.FirstOrDefault(x=> x.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                Console.WriteLine("no user found!");
            }
            return user;
        }

        public bool CreateUser(Users user)
        {
            var users = GetAllUsers();

            if (users == null)
            {
                users.Add(user);
                SaveUser(users);
                return true;

            }
            else if (users.Any(u => u.Email == user.Email))
            {
                Console.WriteLine("user already exists");
                return false;
            }
            else
            {
                users.Add(user);
                SaveUser(users);
                return true;
            }
        }
    }
}
