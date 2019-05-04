using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL.ScriptsT
{
    public class ScriptUpdaterTests
    {
        private readonly ScriptUpdater _scriptUpdater;

        public ScriptUpdaterTests()
        {
            _scriptUpdater = new ScriptUpdater();
        }
        
        [Fact]
        public async Task CreateOrCopyScript_WithoutExistingFile_ReturnError()
        {
            bool res = await _scriptUpdater.CreateOrCopyScriptsAsync(ScriptProvider.Github, "DiplomaSqlScripts",new List<string> {"invalidScriptName.sql"});
            
            res.Should().BeFalse();
            
        }

        [Fact]
        public async Task DeleteScripts_WithoutExistingFile_ReturnsTrue()
        {
            bool res = await _scriptUpdater.DeleteScriptsAsync(ScriptProvider.Github, "DiplomaSqlScripts",
                new List<string> {"invalidScriptName.sql"});

            res.Should().BeTrue();
        }

        [Theory]
        [InlineData("",null, null, false)]
        [InlineData("asd","", null, false)]
        //[InlineData("","", new List<string>(), false)] TODO: change this case to work with empty list        
        public void ValidateInputParams_Invalid_ReturnsFalse(string provider, string repoName, List<string> files, bool expected)
        {
            Enum.TryParse(provider, out ScriptProvider parsed);
            bool res = _scriptUpdater.IsValidInput(parsed, repoName,
                files);

            res.Should().Be(expected);
        }

        [Fact]
        public void IsValidInput_Valid_ReturnsTrue()
        {
            bool res = _scriptUpdater.IsValidInput(ScriptProvider.Github, "ValidRepo",
                new List<string>());

            res.Should().BeTrue();
        }


        [Fact]
        public async Task StartUpdateScripts_InvalidData_ReturnsFalse()
        {
            bool res = await _scriptUpdater.StartUpdateScriptsAsync("",new List<Commit>());
            res.Should().BeFalse();
        }

//        [Fact]
//        public async Task CreateOrCopyScript_FileExists_ReturnOk() //TODO: how to test it, when i run tests from unit tests\bin\Debug folder.
//        {
//            #region Arrange
//
//            string repoPath = @"..\..\..\..\SQLWorker.Web\Scripts\github\DiplomaSqlScripts\"; //TODO: how to test it, when i run tests from unit tests\bin\Debug folder.
//            string repoName = @"DiplomaSqlScripts\";
//            string fullPath = Utilities.GetFullPath(repoPath, "test.sql");
//            ScriptProvider sp = ScriptProvider.Github;
//            
//            if(File.Exists(fullPath))
//                File.Delete(fullPath);
//
//            #endregion
//            
//            
//            bool res = await _scriptWorker.CreateOrCopyScripts(sp, repoName, new[] {"test.sql"});
//
//            res.Should().BeTrue();
//        }
    }
}