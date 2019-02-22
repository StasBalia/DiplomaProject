using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SQLWorker.BLL
{
    public static class ScriptSources
    {
        private static readonly ConcurrentBag<ScriptInfo> Scripts = new ConcurrentBag<ScriptInfo>();
        public static IEnumerable<ScriptInfo> GetAll() => Scripts.ToList();

        public static void Add(ScriptInfo scriptInfo) => Scripts.Add(scriptInfo);

        public static void RemoveAll() => Scripts.Clear();

        public static void AddRange(List<ScriptInfo> list)
        {
            foreach (var scriptInfo in list)
                Scripts.Add(scriptInfo);
        }
    }
}