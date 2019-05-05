using Microsoft.Extensions.Logging;
using SQLWorker.BLL.ProvidersRepositories.Github;
using Xunit;

namespace SQLWorker.UnitTests.BLL.Github
{
    public class GithubPullerTests
    {
        private GithubPuller _puller;
        

        public GithubPullerTests()
        {
            _puller = new GithubPuller(new LoggerFactory().CreateLogger<GithubPuller>());
        }

        [Fact]
        public void AlwaysValid()
        {
            
        }
    }
}