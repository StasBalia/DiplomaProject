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
    public class JsonConverterAndSaverTests
    {
        [Fact]
        public void ConvertToJSON_ReturnCorrectJSON()
        {
            IScriptConverter<string> converter = new JsonConverter();
            string result = converter.ConvertToRightFormat(TestHelper.SimpleDataSet());
            string expected = "{\"Table1\":[{\"colName\":1,\"col1Name\":2},{\"colName\":3,\"col1Name\":4},{\"colName\":5,\"col1Name\":6}]}";
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task SaveToJSON_AlwaysValid()
        {
            IScriptSaver saver = new JsonSaver();
            string result = string.Empty;
            string path = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\";
            string fileName = Utilities.GenerateFileNameForResult("fileScriptXml.sql") + "json";

            await saver.SaveAsync(TestHelper.SimpleDataSet(), path, fileName);
            string fullPath = Path.Combine(path, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
            
        }
    }
}