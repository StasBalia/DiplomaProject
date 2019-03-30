using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using SQLWorker.BLL;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class CsvConverterAndSaverTests
    {
        [Fact]
        public void ConvertToCsv_ReturnCorrectString()
        {
            IScriptConverter<string> converter = new CsvConverter();
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

            string result = converter.ConvertToRightFormat(ds);
            result.Should().NotBeNullOrEmpty();

            string expected = "colName,col1Name\n1,2\n3,4\n5,6\n";
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SaveCsv() //TODO: refactor this with using LaunchInfoClass pls!
        {
            string csv = "colName,col1Name\n1,2\n3,4\n5,6\n";
            IScriptSaver<string> saver = new CsvSaver();
            string pathToSave = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results"; //TODO: remove path!!!
            string fileName = Utilities.GenerateFileNameForResult("fileScript.sql") + "csv";
            
            await saver.SaveAsync(csv, pathToSave, fileName);

            string fullPath = Path.Combine(pathToSave, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
        }
    }
}