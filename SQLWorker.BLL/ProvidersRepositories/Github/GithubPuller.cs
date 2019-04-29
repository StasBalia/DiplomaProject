using System;
using System.IO;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace SQLWorker.BLL.ProvidersRepositories.Github
{
    public class GithubPuller
    {
        private const string USERNAME = "";
        private const string PASSWORD = "";
        private const string EMAIL = "balya.stanislav@gmail.com";
        private const string PATH_TO_REPO = @"..\..\Repos\";
        
        public async Task<bool> PullFromRepo(string fullName)
        {
            string path = Path.Combine(PATH_TO_REPO, fullName);
            string fullPath = new DirectoryInfo(path).FullName;
            try
            {
                Task pull = Task.Factory.StartNew(() => { using (var repo = new Repository(fullPath))
                {
                    // Credential information to fetch
                    PullOptions options = new PullOptions
                    {
                        FetchOptions = new FetchOptions
                        {
                            CredentialsProvider = (url, usernameFromUrl, types) =>
                                new UsernamePasswordCredentials {Username = USERNAME, Password = PASSWORD}
                        }
                    };

                    // User information to create a merge commit
                    var signature = new Signature(
                        new Identity(USERNAME, EMAIL), DateTimeOffset.Now);

                    // Pull
                    Commands.Pull(repo, signature, options);
                }});
                await pull;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}