using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SQLWorker.BLL;
using SQLWorker.BLL.ProvidersRepositories.Github;
using Xunit;

namespace SQLWorker.UnitTests.BLL.Github
{
    public class GithubPullerTests
    {
        private GithubPuller _puller;
        

        public GithubPullerTests()
        {
            _puller = new GithubPuller(new LoggerFactory().CreateLogger<GithubPuller>(), new OptionsWrapper<GitHubCredentials>(new GitHubCredentials()));
        }

        [Fact]
        public void AlwaysValid()
        {
            
        }
    }
}