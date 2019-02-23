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
    }
}