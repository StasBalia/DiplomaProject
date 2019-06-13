using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL.ScriptsT
{
    public class ScriptLoaderTests
    {
        private readonly ScriptLoader _loader;

        public ScriptLoaderTests()
        {
            _loader = new ScriptLoader();
        }

        [Fact]
        public async Task LoadScriptFromFolder_ReturnOk()
        {
            await _loader.LoadScriptsAsync(@"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\");//TODO: remove explicit path !!!!
            ScriptSources.GetAll().Count().Should().Be(12);
            ScriptSources.RemoveAll();
        }

        [Fact]
        public async Task GetFileFromGithubDirectory_ReturnOneFile()
        {
            var result = await _loader.GetFilesFromDirectoryAsync(@"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\", "*.sql", SearchOption.AllDirectories);//TODO: remove explicit path !!!!
            result.Length.Should().Be(12);
            ScriptSources.RemoveAll();
        }

        [Theory]
        [InlineData("Scripts/Github", ScriptProvider.Github)]
        [InlineData("Scripts/github", ScriptProvider.Github)]
        [InlineData("Scripts/svn", ScriptProvider.Svn)]
        [InlineData("Scripts/Svn", ScriptProvider.Svn)]
        [InlineData("incorrect provider", ScriptProvider.Unkown)]
        public void DetermineValueForProvider_ReturnCorrectProvider(string path, ScriptProvider expectedProvider)
        {
            ScriptProvider result = _loader.DetermineProvider(path);
            result.Should().Be(expectedProvider);
        }
    }
}