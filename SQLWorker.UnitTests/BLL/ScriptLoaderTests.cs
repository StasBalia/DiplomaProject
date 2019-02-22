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
            
        }

        [Fact]
        public async Task LoadScriptFromFolder_ReturnOk()
        {
            await _loader.LoadScriptsAsync();

            ScriptSources.GetAll().Count().Should().Be(1);
            
        }
    }
}