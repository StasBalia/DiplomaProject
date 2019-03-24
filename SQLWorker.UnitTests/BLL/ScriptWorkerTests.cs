using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class ExecuteScriptTests
    {
        private readonly ScriptWorker _scriptWorker;
        private readonly IScriptRepository _repository;
        private readonly ScriptLoader _loader;

        private const string DB_CONNECTION_STRING =
            "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=test";

        public ExecuteScriptTests()
        {

            ILoggerFactory factory = new LoggerFactory();
            ILogger<PostgreSqlScriptRepository> log = factory.CreateLogger<PostgreSqlScriptRepository>();
            _loader = new ScriptLoader();
            _repository = new PostgreSqlScriptRepository(DB_CONNECTION_STRING, log); 
            _scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>(), _repository);
        }

        [Fact]
        public async Task AlwaysValidTest() //TODO: мабуть треба тут доробити, а то Assert'a немає
        {
            await _scriptWorker.ExecuteScriptAsync(new LaunchInfo
            {
                PathToScriptFile = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\testScript.sql",
                ParamInfos = new List<ParamInfo>
                {
                    new ParamInfo
                    {
                        Name = "{id}",
                        Value = "1"
                    }
                },
                FileType = "csv"
            });
        }

        public static IEnumerable<object[]> DataSets =>
            new[]
            {
                new object[] {null, false},
                new object[] {new DataSet(), false},
                new object[] {new DataSet {Tables = {new DataTable()}}, true}
            };

        [Theory, MemberData(nameof(DataSets))]
        public void ChecForSuccess_ReturnsCorrectValue(DataSet ds, bool expectedResult)
        {
            bool result = _scriptWorker.CheckForSucces(ds);
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ConvertResultAndSaveToFile_AlwaysValid()
        {
            DataSet ds = new DataSet
            {
                Tables =
                {
                    new DataTable
                    {
                        Columns =
                        {
                            new DataColumn
                            {
                                ColumnName = "colName",
                                DataType = typeof(int)
                            },
                            new DataColumn
                            {
                                ColumnName = "col1Name",
                                DataType = typeof(int)
                            }
                        }
                        
                    }
                }
            };
            var table = ds.Tables[0];
            var dr = table.NewRow();
            dr.ItemArray = new object[] {1, 2};
            table.Rows.Add(dr);
            var dr2 = table.NewRow();
            dr2.ItemArray = new object[] {3, 4};
            table.Rows.Add(dr2);
            var dr3 = table.NewRow();
            dr3.ItemArray = new object[] {5, 6};
            table.Rows.Add(dr3);

            string pathToSave = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results"; //TODO: remove path!!!
            string fileName = Utilities.GenerateFileNameForResult("fileScript");
            FileExtension fileExtension = FileExtension.csv;
            await _scriptWorker.ConvertResultAndSaveToFileAsync(ds, pathToSave, fileName, fileExtension);
        }

        [Theory]
        [InlineData(@"E:\University\Diploma\DiplomaProject\SQLWorker.UnitTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript.sql", 0)]
        [InlineData(@"E:\University\Diploma\DiplomaProject\SQLWorker.UnitTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript1.sql", 1)]
        [InlineData(@"E:\University\Diploma\DiplomaProject\SQLWorker.UnitTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript2.sql", 2)]
        public async Task GetParams_WithCorrectPath_ReturnsParameters(string path, int expectedParams)
        {
            await _loader.LoadScriptsAsync(@"Scripts\github");
            var scriptParams = _scriptWorker.GetParams(path);
            scriptParams.Count.Should().Be(expectedParams);
            ScriptSources.RemoveAll();
        }
    }
}