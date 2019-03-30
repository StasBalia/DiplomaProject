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

        [Theory]
        [InlineData("xlsx", FileExtension.xlsx)]
        [InlineData("csv", FileExtension.csv)]
        [InlineData("asd", default(FileExtension))]
        [InlineData("xml", FileExtension.xml)]
        public void CheckIfFileExtensionExistInEnum_ReturnTrue(string extension, FileExtension convertedExt)
        {
            FileExtension res = Utilities.GetFileExtension(extension);
            res.Should().Be(convertedExt);
        }
    }
}