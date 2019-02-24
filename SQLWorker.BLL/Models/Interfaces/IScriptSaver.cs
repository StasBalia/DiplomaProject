using System.Threading.Tasks;

namespace SQLWorker.BLL.Models.Interfaces
{
    public interface IScriptSaver<T>
    {
        Task SaveAsync(T objectToSave, string pathToSave, string fileName);
    }
}