using System.Data;
using System.Threading.Tasks;
using SQLWorker.DAL.Models;

namespace SQLWorker.DAL.Repositories.Interfaces
{
    public interface IScriptRepository
    {
        ScriptResult ExecuteAndGetResult(string script);
    }
}