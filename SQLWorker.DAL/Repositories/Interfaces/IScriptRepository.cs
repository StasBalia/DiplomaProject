using System.Data;
using System.Threading.Tasks;

namespace SQLWorker.DAL.Repositories.Interfaces
{
    public interface IScriptRepository
    {
        DataSet ExecuteAndGetResult(string script);
    }
}