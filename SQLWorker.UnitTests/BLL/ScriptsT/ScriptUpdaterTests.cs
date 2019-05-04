using System.Threading.Tasks;
using FluentAssertions;
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
            bool res = await _scriptUpdater.CreateOrCopyScripts(ScriptProvider.Github, "DiplomaSqlScripts",new [] {"invalidScriptName.sql"});
            
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