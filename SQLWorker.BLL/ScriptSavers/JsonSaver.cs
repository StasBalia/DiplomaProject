using System.Data;
using System.IO;
using System.Threading.Tasks;
using SQLWorker.BLL.Models.Interfaces;
using SQLWorker.BLL.ScriptConverters;

namespace SQLWorker.BLL.ScriptSavers
{
    public class JsonSaver : IScriptSaver
    {
        private readonly JsonConverter _converter;
        public JsonSaver()
        {
            _converter = new JsonConverter();
        }
        public async Task SaveAsync(DataSet dataSet, string pathToSave, string fileName)
        {
            await File.WriteAllTextAsync(Path.Combine(pathToSave, fileName), _converter.ConvertToRightFormat(dataSet));
        }
    }
}