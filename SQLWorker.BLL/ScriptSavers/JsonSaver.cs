using System.IO;
using System.Threading.Tasks;
using SQLWorker.BLL.Models.Interfaces;

namespace SQLWorker.BLL.ScriptSavers
{
    public class JsonSaver : IScriptSaver<string>
    {
        public async Task SaveAsync(string objectToSave, string pathToSave, string fileName)
        {
            await File.WriteAllTextAsync(Path.Combine(pathToSave, fileName), objectToSave);
        }
    }
}