using System.Data;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SQLWorker.BLL.Models.Interfaces;

namespace SQLWorker.BLL.ScriptSavers
{
    public class XlsSaver : IScriptSaver<XLWorkbook>
    {
        public async Task SaveAsync(XLWorkbook objectToSave, string pathToSave, string fileName)
        {
            await Task.Run(() =>objectToSave.SaveAs(Path.Combine(pathToSave, fileName)));
        }
    }
}