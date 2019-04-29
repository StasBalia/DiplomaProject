using System.Data;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL.SaversAndConverters
{
    public class CsvConverterAndSaverTests
    {
        [Fact]
        public void ConvertToCsv_ReturnCorrectString()
        {
            IScriptConverter<string> converter = new CsvConverter();
            

            string result = converter.ConvertToRightFormat(TestHelper.SimpleDataSet());
            result.Should().NotBeNullOrEmpty();

            string expected = "colName,col1Name\n1,2\n3,4\n5,6\n";
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SaveCsv() //TODO: refactor this with using LaunchInfoClass pls!
        {   
            string csv = "colName,col1Name\n1,2\n3,4\n5,6\n";
            IScriptSaver saver = new CsvSaver();
            string pathToSave = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results"; //TODO: remove path!!!
            string fileName = Utilities.GenerateFileNameForResult("fileScript.sql") + "csv";
            
            await saver.SaveAsync(TestHelper.SimpleDataSet(), pathToSave, fileName);

            string fullPath = Path.Combine(pathToSave, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
        }
    }
}