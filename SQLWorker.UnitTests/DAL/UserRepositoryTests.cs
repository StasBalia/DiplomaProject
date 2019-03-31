using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.DAL.Repositories.Records;
using Xunit;

namespace SQLWorker.UnitTests.DAL
{
    public class UserRepository
    {
        private readonly IUserRepository _repository;
        private const string DB_CONNECTION_STRING =
            "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=test";
        public UserRepository()
        {
            ILoggerFactory factory = new LoggerFactory();
            _repository = new PostgreSqlUserRepository(DB_CONNECTION_STRING, factory.CreateLogger<PostgreSqlUserRepository>());
            //_scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>());
        }

        [Fact]
        public async Task SaveUser_CorrectData_ReturnsUserId()
        {
            string param = new string(Guid.NewGuid().ToString().Take(15).ToArray());
            User userData = new User
            {
                Name = param,
                Email = param,
                Password = param
            };
            
            var res = await _repository.SaveUserAsync(userData);

            Assert.True(res > 0);
        }

        [Fact]
        public async Task GetUserByName_CorrectUserName_ReturnsUser()
        {
            User expectedUser = new User
            {
                Name = "Stanislav",
                Email = "stas@gmail.com",
                Password = "黧蹁扈椵廗笪급瞛⚱䖤ﾹ욝骘禠"
            };
            string email = "stas@gmail.com";
            var res = await _repository.GetUserByEmailAsync(email);
            
            res.Should().BeEquivalentTo(expectedUser);
        }
    }
}