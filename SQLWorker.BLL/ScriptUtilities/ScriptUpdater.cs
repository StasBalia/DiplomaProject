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
                    var fileFrom = Directory.GetFiles(pathFrom, file, SearchOption.AllDirectories).FirstOrDefault(x => x.Contains(file));
                    string content = await File.ReadAllTextAsync(fileFrom);
                    string fileTo = Path.Combine(pathTo, file);
                    if (File.Exists(fileTo))
                        await File.AppendAllTextAsync(fileTo, content);
                    else
                    {
                        Directory.CreateDirectory(pathTo);
                        using (var wr = File.Create(fileTo))
                        {
                            await wr.WriteAsync(Encoding.UTF8.GetBytes(content));
                        }
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
                        string fileTo = Directory.GetFiles(pathTo, file, SearchOption.AllDirectories).FirstOrDefault(x => x.Contains(file));
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

        public bool IsValidInput(ScriptProvider provider, string repositoryName, List<string> fileNames)
        {
            if (!Enum.IsDefined(typeof(ScriptProvider), provider))
                return false;
            if (string.IsNullOrEmpty(repositoryName))
            {
                _log.LogWarning("Repository name is null.", repositoryName);
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
                return await CreateOrCopyScriptsAsync(ScriptProvider.Github, repositoryName, commit?.Added) &&
                       await CreateOrCopyScriptsAsync(ScriptProvider.Github, repositoryName, commit?.Modified) &&
                       await DeleteScriptsAsync(ScriptProvider.Github, repositoryName, commit?.Removed);
            }
            catch (Exception e)
            {
                _log.LogError("Something went wrong. Ex: {@e}", e);
                return await Task.FromResult(false);
            }
        }
    }
}