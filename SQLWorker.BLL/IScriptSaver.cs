using System.Data;
using System.Threading.Tasks;

namespace SQLWorker.BLL
{
    public interface IScriptSaver<T>
    {
        Task SaveAsync(T objectToSave, string pathToSave, string fileName);
    }

    public interface IScriptConverter<T>
    {
        T ConvertToRightFormat(DataSet result);
    }
}