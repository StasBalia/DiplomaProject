using System.Data;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;

namespace SQLWorker.BLL.ScriptSavers
{
    public class XlsSaver : IScriptSaver
    {
        private readonly XlsConverter _converter;
        public XlsSaver()
        {
            _converter = new XlsConverter();
        }
        public async Task SaveAsync(DataSet dataSet, string pathToSave, string fileName)
        {
            await Task.Run(() =>
            {
                var res = _converter.ConvertToRightFormat(dataSet);
                res.SaveAs(Path.Combine(pathToSave, fileName));
                res.Dispose();
            });
        }
    }
}