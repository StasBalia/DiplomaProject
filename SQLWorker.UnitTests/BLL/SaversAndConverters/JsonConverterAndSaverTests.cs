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

            string result = converter.ConvertToRightFormat(ds);
            string expected = "{\"TableName\":[{\"colName\":1,\"col1Name\":2},{\"colName\":3,\"col1Name\":4},{\"colName\":5,\"col1Name\":6}]}";
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task SaveToJSON_AlwaysValid()
        {
            IScriptSaver<string> saver = new JsonSaver();
            string result = string.Empty;
            string path = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\";
            string fileName = Utilities.GenerateFileNameForResult("fileScriptXml.sql") + "json";

            await saver.SaveAsync(result, path, fileName);
            string fullPath = Path.Combine(path, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
            
        }
    }
}