using System.Data;

namespace SQLWorker.BLL.Models.Interfaces
{
    public interface IScriptConverter<T>
    {
        T ConvertToRightFormat(DataSet result);
    }
}