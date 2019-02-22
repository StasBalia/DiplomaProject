using System.Data;
using System.Threading.Tasks;

namespace SQLWorker.DAL.Repositories.Interfaces
{
    public interface IScriptRepository
    {
        object ExecuteAndGetResult(string script);
    }
}