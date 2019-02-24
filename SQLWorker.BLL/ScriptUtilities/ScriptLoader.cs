using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SQLWorker.BLL.Models;

namespace SQLWorker.BLL.ScriptUtilities
{
    public class ScriptLoader
    {
        public async Task LoadScriptsAsync(string path)
        {
            var files = await GetFilesFromDirectoryAsync(path, "*.sql",
                SearchOption.AllDirectories);
            
            List<ScriptInfo> scriptInfo = new List<ScriptInfo>();

            foreach (var file in files)
            {
                var fileContent = File.ReadAllText(file.FullName);
                scriptInfo.Add(new ScriptInfo
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Provider = DetermineProvider(file.FullName),
                    Parameters = Regex.Matches(fileContent, "{.+?}").Select(match => match.Value)
                        .Distinct().ToList()
                });
            }
            ScriptSources.AddRange(scriptInfo);
        }

        public async Task<FileInfo[]> GetFilesFromDirectoryAsync(string pathToDirectory, string searchPattern, SearchOption searchOption)
        {
            DirectoryInfo directory = new DirectoryInfo(pathToDirectory);
            return await Task.Run(() => directory.GetFiles(searchPattern, searchOption));
        }

        public string DetermineProvider(string path)
        {
            string lowerCasePath = path.ToLower();
            return lowerCasePath.Contains("git") || lowerCasePath.Contains("github") ? "github" :
                lowerCasePath.Contains("svn")? "svn" : string.Empty;
        }
    }
}