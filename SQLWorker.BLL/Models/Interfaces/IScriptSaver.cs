using System.Data;
using System.Threading.Tasks;

namespace SQLWorker.BLL.Models.Interfaces
{
    public interface IScriptSaver
    {
        Task SaveAsync(DataSet dataSet, string pathToSave, string fileName);
    }
}