using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SQLWorker.BLL.ScriptUtilities;

namespace SQLWorker.BLL.ProvidersRepositories.Github
{
    public class GithubPuller
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly string _email;
        private const string PATH_TO_REPO = @"..\..\Repos\";
        private readonly ILogger _log;

        public GithubPuller(GitHubCredentials creds)
        {
            _userName = creds.UserName;
            _password = creds.Password;
            _email = creds.Email;
        }
        public GithubPuller(ILogger<GithubPuller> log, IOptions<GitHubCredentials> options)
        {
            _log = log;
            _userName = options.Value.UserName;
            _password = options.Value.Password;
            _email = options.Value.Email;
        }
        
        public async Task<bool> PullFromRepoAsync(string urlToRepo, string repositoryName)
        {
            try
            {
                Task pull = Task.Factory.StartNew(() =>
                {
                    string pathToRepo = Utilities.GetFullPath(PATH_TO_REPO, repositoryName);
                    if(!Directory.Exists(pathToRepo))
                        GitClone(urlToRepo, pathToRepo);
                    else
                    {
                        using (var repo = new Repository(pathToRepo))
                        {
                            // Credential information to fetch
                            PullOptions options = new PullOptions
                            {
                                FetchOptions = new FetchOptions
                                {
                                    CredentialsProvider = (url, usernameFromUrl, types) =>
                                        new UsernamePasswordCredentials {Username = _userName, Password = _password}
                                }
                            };

                            // User information to create a merge commit
                            var signature = new Signature(
                                new Identity(_userName, _email), DateTimeOffset.Now);

                            // Pull
                            Commands.Pull(repo, signature, options);
                        }    
                    }
                });
                await pull;
                return true;
            }
            catch (Exception e)
            {
                _log.LogError("Can't pull from repository. {@e}",e);
                return false;
            }
        }

        public void PullFromRepositories(string[] repositoriesNames)
        {
            var names = repositoriesNames.Select(x => x.Split('\\').LastOrDefault()).ToList();
            foreach (var name in names)
            {
                PullFromRepoAsync(string.Empty, name).Wait();
            }
        }
        private void GitClone(string urlToRepo, string pathToRepo)
        {
            Directory.CreateDirectory(pathToRepo);
            Credentials s = new UsernamePasswordCredentials
            {
                Username = _userName, Password = _password
            };
            Repository.Clone(urlToRepo, pathToRepo, new CloneOptions{ CredentialsProvider = (url, user, cred) => s});
        }
    }
}