using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.DAL.Repositories.Records;
using SQLWorker.UnitTests.DAL.Utilities;
using Xunit;

namespace SQLWorker.UnitTests.DAL
{
    public class UserRepository
    {
        private readonly IUserRepository _repository;
        public UserRepository()
        {
            ILoggerFactory factory = new LoggerFactory();
            _repository = new PostgreSqlUserRepository(ConnectionStringProvider.DB_CONNECTION_STRING, factory.CreateLogger<PostgreSqlUserRepository>());
            //_scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>());
        }

        [Fact]
        [UseDatabase(ConnectionStringProvider.DB_CONNECTION_STRING)]
        public void SaveUser_CorrectData_ReturnsUserId()
        {
            string param = new string(Guid.NewGuid().ToString().Take(15).ToArray());
            User userData = new User
            {
                Name = param,
                Email = param,
                Password = param
            };
            
            var res = _repository.SaveUserAsync(userData).Result;

            Assert.True(res > 0);
        }

        [Fact]
        [UseDatabase(ConnectionStringProvider.DB_CONNECTION_STRING)]
        public void  GetUserByName_CorrectUserName_ReturnsUser()
        {
            User expectedUser = new User
            {
                Name = "Stanislav",
                Email = "stas@gmail.com",
                Password = "黧蹁扈椵廗笪급瞛⚱䖤ﾹ욝骘禠"
            };
            string email = "stas@gmail.com";
            var res = _repository.GetUserByEmailAsync(email).Result;
            
            res.Should().BeEquivalentTo(expectedUser);
        }
    }
}