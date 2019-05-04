using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL.ScriptUtilities
{
    public class ScriptUpdater
    {
        private const string PATH_TO_REPO = @"..\..\Repos\";
        private const string PATH_TO_SAVE = @"..\SQLWorker.Web\Scripts\";
        
        public async Task<bool> CreateOrCopyScriptsAsync(ScriptProvider provider, string repositoryName, string[] fileNames)
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
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteScriptsAsync(ScriptProvider provider, string repositoryName, string[] fileNames)
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
                        //TODO: log that file was not found
                    }
                }
                
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public bool IsValidInput(ScriptProvider provider, string repositoryName, string[] fileNames)
        {
            if (!Enum.IsDefined(typeof(ScriptProvider), provider))
                return false;
            if (string.IsNullOrEmpty(repositoryName))
                return false; //TODO: need to write error to log
            if(fileNames == null)   
                return false; //TODO: need to write error to log
            
            return true;
        }
    }
}