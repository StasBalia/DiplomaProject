using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentAssertions;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL
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
                        },
                        TableName = "TableName"
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

            XLWorkbook result = converter.ConvertToRightFormat(ds);

            var resultList = TestHelper.ExtractSpecificDataFromWorksheet(result.Worksheet("ScriptResult"));
            var expectedList = TestHelper.ExtractSpecificDataFromWorksheet(wb.Worksheet("ScriptResult"));
            resultList.Should().BeEquivalentTo(expectedList);
            result.Dispose();
            wb.Dispose();
        }

       


        [Fact]
        public async Task SaveXls()
        {
            IScriptSaver<XLWorkbook> saver = new XlsSaver();
            IScriptConverter<XLWorkbook> converter = new XlsConverter();

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
                        },
                        TableName = "TableName"
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

            XLWorkbook result = converter.ConvertToRightFormat(ds);
            string path = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\";
            string fileName = Utilities.GenerateFileNameForResult("fileScriptXml.sql") + "xlsx";

            await saver.SaveAsync(result, path, fileName);
            string fullPath = Path.Combine(path, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
            result.Dispose();
        }
    }
}