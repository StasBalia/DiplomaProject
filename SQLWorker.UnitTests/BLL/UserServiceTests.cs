using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.DAL.Repositories.Records;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _repository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            ILoggerFactory factory = new LoggerFactory();
            _repository = new Mock<IUserRepository>();
            _userService = new UserService(factory.CreateLogger<UserService>(), _repository.Object);
        }

        public static IEnumerable<object[]> DataSets => 
            new[]
            {
                new object[] {1, new UserDTO
                {
                    Id = 1,
                    Name = "userName",
                    Email = "email",
                    Password = "Ǣ崆吅♥쌕쀠ᴊ졛쫭鵆쉲๹ᔤభ⬞襡"
                }},
                new object[] {0, new UserDTO()},
                new object[] {-1, new UserDTO()}
            };
        [Theory]
        [MemberData(nameof(DataSets)) ]
        public async Task AddUser_ReturnsCorrectedUser(long expectedId, UserDTO expectedDto) 
        {
            _repository.Setup(x => x.SaveUserAsync(It.IsAny<User>())).Returns(Task.FromResult<long>(expectedId));
            UserDTO expected = expectedDto;
            UserDTO result = await _userService.AddAsync("userName", "password", "email");
            result.Should().BeEquivalentTo(expected);
        }
    }
}