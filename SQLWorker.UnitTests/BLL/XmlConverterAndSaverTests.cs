using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using FluentAssertions;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;
using SQLWorker.BLL.ScriptSavers;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class XmlConverterAndSaverTests
    {
        [Fact]
        public void ConvertToXml_ReturnCorrectXml()
        {
            IScriptConverter<string> converter = new XmlConverter();
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
            result.Should().NotBeNullOrEmpty();

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
        public async Task SaveXml()
        {
            string xml = string.Empty;
            IScriptSaver<string> saver = new XmlSaver();
            string path = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\";
            string fileName = Utilities.GenerateFileNameForResult("fileScriptXml.sql") + "xml";

            await saver.SaveAsync(xml, path, fileName);
            string fullPath = Path.Combine(path, fileName);
            bool isExist = File.Exists(fullPath);
            isExist.Should().BeTrue();
        }
    }
}