using System;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.UnitTests.DAL.Utilities;
using Xunit;

namespace SQLWorker.UnitTests.DAL
{
    public class ScriptRepositoryTests
    {
        private readonly IScriptRepository _repository;
        public ScriptRepositoryTests()
        {
            ILoggerFactory factory = new LoggerFactory();
            _repository = new PostgreSqlScriptRepository(ConnectionStringProvider.DB_CONNECTION_STRING, factory.CreateLogger<PostgreSqlScriptRepository>());
            //_scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>());
        }

        [Fact]
        [UseDatabase(ConnectionStringProvider.DB_CONNECTION_STRING)]
        public void ExecuteScript_Valid()
        {
            
            var res = _repository.ExecuteAndGetResult("SELECT * FROM public.usertable WHERE id = 1");

            res.Start.Should().NotBe(default(DateTime));
            res.End.Should().NotBe(default(DateTime));
            res.Error.Should().BeEquivalentTo(default(string));
            res.DataSet.Should().NotBeNull();
        }
    }
}