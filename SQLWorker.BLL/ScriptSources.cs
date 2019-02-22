using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SQLWorker.BLL
{
    public class ScriptSources
    {
        private static ConcurrentBag<ScriptInfo> _scripts = new ConcurrentBag<ScriptInfo>();
        public static IEnumerable<ScriptInfo> GetAll() => _scripts.ToList();

        public static void Add(ScriptInfo scriptInfo) => _scripts.Add(scriptInfo);

        public static void RemoveAll() => _scripts.Clear();

        public static void AddRange(List<ScriptInfo> list)
        {
            foreach (var scriptInfo in list)
                _scripts.Add(scriptInfo);
        }
    }
}