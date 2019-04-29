using System.Data;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using FluentAssertions;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL.SaversAndConverters
{
    public class XlsConverterAndSaverTests
    {
        [Fact]
        public void ConvertToExcelForm_ReturnCorrectWorkBook()
        {
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add("ScriptResult");
            var res = wb.Worksheets.Worksheet("ScriptResult");
            res.Cell("A1").Value = "colName";
            res.Cell("A2").Value = 1;
            res.Cell("A3").Value = 3;
            res.Cell("A4").Value = 5;
            res.Cell("B1").Value = "colName";
            res.Cell("B2").Value = 2;
            res.Cell("B3").Value = 4;
            res.Cell("B4").Value = 6;
            
            IScriptConverter<XLWorkbook> converter = new XlsConverter();
            XLWorkbook result = converter.ConvertToRightFormat(TestHelper.SimpleDataSet());

            var resultList = TestHelper.ExtractSpecificDataFromWorksheet(result.Worksheet("ScriptResult"));
            var expectedList = TestHelper.ExtractSpecificDataFromWorksheet(wb.Worksheet("ScriptResult"));
            resultList.Should().BeEquivalentTo(expectedList);
            result.Dispose();
            wb.Dispose();
        }

       


        [Fact]
        public async Task SaveXls_AlwaysValid()
        {
            IScriptSaver saver = new XlsSaver();
            string path = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\";
            string fileName = Utilities.GenerateFileNameForResult("fileScriptXml.sql") + "xlsx";

            await saver.SaveAsync(TestHelper.SimpleDataSet(), path, fileName);
            
            string fullPath = Path.Combine(path, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
        }
    }
}