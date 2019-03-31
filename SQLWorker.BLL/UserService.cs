using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL.Models;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.DAL.Repositories.Records;

namespace SQLWorker.BLL
{
    public class UserService
    {

        private ILogger<UserService> _log;
        private readonly IUserRepository _userRepository;
        
        public UserService(ILogger<UserService> log, IUserRepository repository)
        {
            _log = log;
            _userRepository = repository;
        }
        
        private string HashString(string str)
        {
            var message = Encoding.Unicode.GetBytes(str);
            var hash = new SHA256Managed();

            var hashValue = hash.ComputeHash(message);
            return Encoding.Unicode.GetString(hashValue);
        }

        public async Task<UserDTO> AddAsync(string userName, string password, string email)
        {
            string hashedPassword = HashString(password);
            long id = await _userRepository.SaveUserAsync(
                new User {Name = userName, Email = email.ToLower(), Password = hashedPassword});

            if (id <= 0)
                return await Task.FromResult(new UserDTO());
            
            UserDTO user = new UserDTO
            {
                Id = id,
                Password = hashedPassword,
                Name = userName,
                Email = email
            };
            return await Task.FromResult(user);
        }

        public async Task<User> AuthenticateAsync(string userName, string password)
        {
            string hashedPassword = HashString(password);
            User userData = await _userRepository.GetUserByEmailAsync(userName.ToLower());
            
            if (userData == null)
                return await Task.FromResult<User>(null);

            if (!hashedPassword.Equals(userData.Password))
                return await Task.FromResult<User>(null);

            return await Task.FromResult(userData);
        }
    }
}