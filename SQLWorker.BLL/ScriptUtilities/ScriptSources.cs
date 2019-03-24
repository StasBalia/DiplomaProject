using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SQLWorker.BLL.Models;

namespace SQLWorker.BLL.ScriptUtilities
{
    public static class ScriptSources
    {
        private static  object objLock = new object();
        private static readonly ConcurrentBag<ScriptInfo> Scripts = new ConcurrentBag<ScriptInfo>();
        public static IEnumerable<ScriptInfo> GetAll() => Scripts.ToList();

        public static void Add(ScriptInfo scriptInfo) => Scripts.Add(scriptInfo);

        public static void RemoveAll()
        {
            lock (objLock)
            {
                Scripts.Clear();    
            }
            
        }

        public static void AddRange(List<ScriptInfo> list)
        {
            lock (objLock)
            {
                foreach (var scriptInfo in list)
                    Scripts.Add(scriptInfo);    
            }
            
        }

        public static ScriptInfo GetSingleScriptByFilePath(string filePath)
        {
            return GetAll().FirstOrDefault(x => x.Path.Equals(filePath));
        }
    }
}