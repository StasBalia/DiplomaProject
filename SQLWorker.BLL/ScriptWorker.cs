using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SQLWorker.BLL
{
    public class ScriptWorker
    {
        private readonly ILogger _log;
        
        public ScriptWorker(ILogger log)
        {
            _log = log;
        }

        public async Task<object> ExecuteScript(LaunchInfo launchInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}