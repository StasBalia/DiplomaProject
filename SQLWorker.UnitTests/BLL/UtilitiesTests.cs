using FluentAssertions;
using SQLWorker.BLL;
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
            string fileExtension = "csv";
            FileExtension res = Utilities.GetFileExtension(fileExtension);
            res.Should().Be(FileExtension.csv);
        }
    }
}