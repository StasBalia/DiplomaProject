using SQLWorker.BLL.ProvidersRepositories.Github;
using Xunit;

namespace SQLWorker.UnitTests.BLL.Github
{
    public class GithubPullerTests
    {
        private GithubPuller _puller;

        public GithubPullerTests()
        {
            _puller = new GithubPuller();
        }

        [Fact]
        public void AlwaysValid()
        {
            _puller = new GithubPuller();
        }
    }
}