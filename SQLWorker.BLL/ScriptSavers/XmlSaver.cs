using System.Data;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;

namespace SQLWorker.BLL.ScriptSavers
{
    public class XmlSaver : IScriptSaver
    {
        private readonly XmlConverter _converter;
        public XmlSaver()
        {
            _converter = new XmlConverter();
        }
        public async Task SaveAsync(DataSet dataSet, string pathToSave, string fileName)
        {
            await File.WriteAllTextAsync(Path.Combine(pathToSave + fileName), _converter.ConvertToRightFormat(dataSet));
            
        }
    }
}