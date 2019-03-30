using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using ClosedXML.Excel;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class ExecuteScriptTests
    {
        private readonly ScriptWorker _scriptWorker;
        private readonly IScriptRepository _repository;
        private readonly ScriptLoader _loader;

        private const string DB_CONNECTION_STRING =
            "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=test";

        public ExecuteScriptTests()
        {
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PostgreSqlScriptRepository> log = factory.CreateLogger<PostgreSqlScriptRepository>();
            _loader = new ScriptLoader();
            _repository = new PostgreSqlScriptRepository(DB_CONNECTION_STRING, log);
            _scriptWorker = new ScriptWorker(factory.CreateLogger<ScriptWorker>(), _repository);
        }

        [Fact]
        public async Task AlwaysValidTest() //TODO: мабуть треба тут доробити, а то Assert'a немає
        {
            await _scriptWorker.ExecuteScriptAsync(new LaunchInfo
            {
                PathToScriptFile = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\testScript.sql",
                ParamInfos = new List<ParamInfo>
                {
                    new ParamInfo
                    {
                        Name = "{id}",
                        Value = "1"
                    }
                },
                FileType = "csv"
            });
        }

        public static IEnumerable<object[]> DataSets =>
            new[]
            {
                new object[] {null, false},
                new object[] {new DataSet(), false},
                new object[] {new DataSet {Tables = {new DataTable()}}, true}
            };

        [Theory, MemberData(nameof(DataSets))]
        public void ChecForSuccess_ReturnsCorrectValue(DataSet ds, bool expectedResult)
        {
            bool result = _scriptWorker.CheckForSucces(ds);
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ConvertResultToCSVAndSaveToFile_AlwaysValid()
        {
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
                        }
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

            string pathToSave =
                @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\"; //TODO: remove path!!!
            string fileName = Utilities.GenerateFileNameForResult("fileScript") + "csv";
            FileExtension fileExtension = FileExtension.csv;
            await _scriptWorker.ConvertResultAndSaveToFileAsync(ds, pathToSave, fileName, fileExtension);
        }

        [Theory]
        [InlineData(
            @"E:\University\Diploma\DiplomaProject\SQLWorker.UnitTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript.sql",
            0)]
        [InlineData(
            @"E:\University\Diploma\DiplomaProject\SQLWorker.UnitTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript1.sql",
            1)]
        [InlineData(
            @"E:\University\Diploma\DiplomaProject\SQLWorker.UnitTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript2.sql",
            2)]
        public async Task GetParams_WithCorrectPath_ReturnsParameters(string path, int expectedParams)
        {
            await _loader.LoadScriptsAsync(@"Scripts\github");
            var scriptParams = _scriptWorker.GetParams(path);
            scriptParams.Count.Should().Be(expectedParams);
            ScriptSources.RemoveAll();
        }


        [Fact]
        public async Task ConvertResultToXmlAndSaveFile_AlwaysValid()
        {
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
            string pathToSave =
                @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\"; //TODO: remove path!!!
            string fileName = Utilities.GenerateFileNameForResult("fileScript") + "xml";
            FileExtension fileExtension = FileExtension.xml;
            await _scriptWorker.ConvertResultAndSaveToFileAsync(ds, pathToSave, fileName, fileExtension);
            string result =
                File.ReadAllText(Path.Combine(
                    @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results", fileName));

            XmlDocument xmlDoc1 = new XmlDocument();
            XmlDocument xmlDoc2 = new XmlDocument();
            xmlDoc2.LoadXml(result);
            xmlDoc1.LoadXml("<NewDataSet>" +
                            "  <xs:schema id=\"NewDataSet\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">" +
                            "   <xs:element name=\"NewDataSet\" msdata:IsDataSet=\"true\" msdata:MainDataTable=\"TableName\" msdata:UseCurrentLocale=\"true\">" +
                            "     <xs:complexType>" +
                            "       <xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">" +
                            "         <xs:element name=\"TableName\">" +
                            "           <xs:complexType>" +
                            "             <xs:sequence>" +
                            "                <xs:element name=\"colName\" type=\"xs:int\" minOccurs=\"0\" />" +
                            "               <xs:element name=\"col1Name\" type=\"xs:int\" minOccurs=\"0\" />" +
                            "             </xs:sequence>" +
                            "           </xs:complexType>" +
                            "         </xs:element>" +
                            "       </xs:choice>" +
                            "     </xs:complexType>" +
                            "   </xs:element>" +
                            " </xs:schema>" +
                            " <TableName>" +
                            "   <colName>1</colName>" +
                            "   <col1Name>2</col1Name>" +
                            " </TableName>" +
                            " <TableName>" +
                            "   <colName>3</colName>" +
                            "   <col1Name>4</col1Name>" +
                            " </TableName>" +
                            " <TableName>" +
                            "   <colName>5</colName>" +
                            "   <col1Name>6</col1Name>" +
                            " </TableName>" +
                            "</NewDataSet>");

            xmlDoc2.OuterXml.Should().BeEquivalentTo(xmlDoc1.OuterXml);
        }


        [Fact]
        public async Task ConvertToXlsxAndSaveFile_AlwaysValid()
        {
            XLWorkbook expectedWorkbook = new XLWorkbook();
            expectedWorkbook.Worksheets.Add("ScriptResult");
            var res = expectedWorkbook.Worksheets.Worksheet("ScriptResult");
            res.Cell("A1").Value = "colName";
            res.Cell("A2").Value = 1;
            res.Cell("A3").Value = 3;
            res.Cell("A4").Value = 5;
            res.Cell("B1").Value = "colName";
            res.Cell("B2").Value = 2;
            res.Cell("B3").Value = 4;
            res.Cell("B4").Value = 6;

           
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

            string pathToSave =
                @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\"; //TODO: remove path!!!
            string fileName = Utilities.GenerateFileNameForResult("fileScript") + "xlsx";
            FileExtension fileExtension = FileExtension.xlsx;
            
            await _scriptWorker.ConvertResultAndSaveToFileAsync(ds, pathToSave, fileName, fileExtension);

            XLWorkbook resultWorkbook = new XLWorkbook(Path.Combine(pathToSave, fileName));
            var resultList = TestHelper.ExtractSpecificDataFromWorksheet(resultWorkbook.Worksheet("ScriptResult"));
            var expectedList = TestHelper.ExtractSpecificDataFromWorksheet(expectedWorkbook.Worksheet("ScriptResult"));

            resultList.Should().BeEquivalentTo(expectedList);
            resultWorkbook.Dispose();
            expectedWorkbook.Dispose();
        }
    }
}