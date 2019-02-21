using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SQLWorker.Web;
using Xunit;

namespace SQLWorker.Tests.IntegrationTests
{
    public class EndToEnd : IClassFixture<WebApplicationFactory<Startup>>
    {
        private WebApplicationFactory<Startup> _factory;
        
        public EndToEnd(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task BasicPingTest_ReturnTrue()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/Home/Index");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }
    }
}