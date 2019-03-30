using System.Data;
using ClosedXML.Excel;
using SQLWorker.BLL.Models.Interfaces;

namespace SQLWorker.BLL.ScriptConverters
{
    public class XlsConverter : IScriptConverter<XLWorkbook>
    {
        public XLWorkbook ConvertToRightFormat(DataSet result)
        {
            XLWorkbook workbook = new XLWorkbook();
            workbook.Worksheets.Add(result.Tables[0], "ScriptResult");
            return workbook;
        }
    }
}