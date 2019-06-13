using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.Models.Enums;


namespace SQLWorker.BLL.ScriptUtilities
{
    public class ScriptUpdater
    {
        private const string PATH_TO_REPO = @"..\..\Repos\";
        private const string PATH_TO_SAVE = @"..\SQLWorker.Web\Scripts\";
        private ILogger _log;
        public ScriptUpdater()
        {
            
        }

        public ScriptUpdater(ILogger<ScriptUpdater> log)
        {
            _log = log;
        }
        
        public async Task<bool> CreateOrCopyScriptsAsync(ScriptProvider provider, string repositoryName, List<string> fileNames)
        {
            try
            {
                if (!IsValidInput(provider, repositoryName, fileNames))
                    return await Task.FromResult(false); 
                string pathTo = Utilities.GetFullPath(PATH_TO_SAVE,  $@"{provider.ToString().ToLower()}\" + repositoryName);
               
                
                string pathFrom = Utilities.GetFullPath(PATH_TO_REPO, repositoryName);
                
                foreach (var file in fileNames)
                {
                    var fileName = file.Split('/').LastOrDefault();
                    var fileFrom = Directory.GetFiles(pathFrom, file, SearchOption.AllDirectories).FirstOrDefault(x => x.Contains(fileName));
                    string content = await File.ReadAllTextAsync(fileFrom);
                    string fileTo = Path.Combine(pathTo, file).Replace('/','\\');
                  
                    if (File.Exists(fileTo))
                        await File.WriteAllTextAsync(fileTo, content);
                    else
                    {
                        Directory.CreateDirectory(pathTo);
                        await File.WriteAllTextAsync(fileTo, content);
                    }
                }
            
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                _log.LogError("Something went wrong. Ex: {@e}", e);
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteScriptsAsync(ScriptProvider provider, string repositoryName, List<string> fileNames)
        {
            try
            {
                if (!IsValidInput(provider, repositoryName, fileNames))
                    return await Task.FromResult(false);
                
                string pathTo = Utilities.GetFullPath(PATH_TO_SAVE,  $@"{provider.ToString().ToLower()}\" + repositoryName);
                
                
                foreach (var file in fileNames)
                {
                    try
                    {
                        var fileName = file.Replace('/', '\\');
                        string fileTo = GetFullFilePath(fileName, pathTo);
                        File.Delete(fileTo);
                    }
                    catch (Exception e)
                    {
                        _log.LogError("File {@file} not found for delete.", file);
                    }
                }
                
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                _log.LogError("Something went wrong. Ex: {@e}", e);
                return await Task.FromResult(false);
            }
        }
        private string GetFullFilePath(string fileName, string pathTo) => Directory.GetFiles(pathTo, fileName, SearchOption.AllDirectories).FirstOrDefault(x => x.Contains(fileName));

        public bool IsValidInput(ScriptProvider provider, string repositoryName, List<string> fileNames)
        {
            if (!Enum.IsDefined(typeof(ScriptProvider), provider))
                return false;
            if (string.IsNullOrEmpty(repositoryName))
            {
                _log.LogWarning("File doesn't exists.", repositoryName);
                return false;
            }

            if (fileNames != null) return true;
            
            _log.LogWarning("File array is empty - {@repositoryName}", repositoryName);
            return false;

        }

        public async Task<bool> StartUpdateScriptsAsync(string repositoryName, List<Commit> commits)
        {
            try
            {
                Commit commit = commits.OrderByDescending(x => x.TimeStamp).FirstOrDefault();
                bool res1 = true, res2 = true, res3 = true;
                if (commit?.Added.Count > 0)
                    res1 = await CreateOrCopyScriptsAsync(ScriptProvider.Github, repositoryName, commit.Added);
                if (commit?.Modified.Count > 0)
                    res2 = await CreateOrCopyScriptsAsync(ScriptProvider.Github, repositoryName, commit.Modified);
                if (commit?.Removed.Count > 0)
                    res3 = await DeleteScriptsAsync(ScriptProvider.Github, repositoryName, commit.Removed);
                return await Task.FromResult(res1 == res2 == res3);
            }
            catch (Exception e)
            {
                _log.LogError("Something went wrong. Ex: {@e}", e);
                return await Task.FromResult(false);
            }
        }
    }
}