using System.IO;
using FluentAssertions;
using SQLWorker.BLL;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class UtilitiesTests
    {
        [Fact]
        public void GenerateFileName_ReturnCorrectFileName()
        {
            string scriptName = "fileScript.sql";
            string res = Utilities.GenerateFileNameForResult(scriptName);
            res.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void CheckIfFileExtensionExistInEnum_ReturnTrue()
        {
            var t = new DirectoryInfo("Scripts/github").FullName;
            string fileExtension = "csv";
            FileExtension res = Utilities.GetFileExtension(fileExtension);
            res.Should().Be(FileExtension.csv);
        }
    }
}