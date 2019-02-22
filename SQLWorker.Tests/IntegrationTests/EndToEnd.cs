using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SQLWorker.Web;
using SQLWorker.Web.Controllers;
using SQLWorker.Web.Models.Request;
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

        [Fact]
        public async Task WebHookPing_ReturnTrue()
        {
            var client = _factory.CreateClient();
            var p = new PushEvent
            {
                Repository = new Repository
                {
                    Name = "a",
                    FullName = "b"
                },
                Ref = "m/a"
                ,Commits = new List<Commit>
                {
                    new Commit
                    {
                        Author = new Author
                        {
                            Email = "email",
                            Name = "Stas"
                        },
                        Distinct = true,
                        Message = "message",
                        SHA = "sha",
                        Url = "urlik",
                        TimeStamp = DateTime.Now.ToUniversalTime()
                    }
                }
            };
            var response = await client.PostAsJsonAsync("/Github/Payload", p);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }
    }
}