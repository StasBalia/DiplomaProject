using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SQLWorker.BLL;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class ScriptLoaderTests
    {
        private ScriptLoader _loader;

        public ScriptLoaderTests()
        {
            _loader = new ScriptLoader();
        }

        [Fact]
        public async Task LoadScriptFromFolder_ReturnOk()
        {
            await _loader.LoadScriptsAsync(@"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\");//TODO: remove explicit path !!!!
            ScriptSources.GetAll().Count().Should().Be(1);
        }

        [Fact]
        public async Task GetFileFromGithubDirectory_ReturnOneFile()
        {
            var result = await _loader.GetFilesFromDirectoryAsync(@"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\", "*.sql", SearchOption.AllDirectories);//TODO: remove explicit path !!!!
            result.Length.Should().Be(1);
        }

        [Theory]
        [InlineData("Scripts/Github", "github")]
        [InlineData("Scripts/svn", "svn")]
        public void DetermineValueForProvider_ReturnCorrectProvider(string path, string expectedProvider)
        {
            string result = _loader.DetermineProvider(path);
            result.Should().Be(expectedProvider);
        }
    }
}