using System.Data;
using System.Threading.Tasks;

namespace SQLWorker.BLL
{
    public interface IScriptSaver<T>
    {
        T ConvertToRightFormat(DataSet result);
        Task SaveAsync(T objectToSave, string pathToSave, string fileName);
    }
}