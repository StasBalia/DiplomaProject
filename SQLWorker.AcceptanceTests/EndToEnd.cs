using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SQLWorker.Web;
using SQLWorker.Web.Models.Request.Github;
using SQLWorker.Web.Models.Request.Script;
using Xunit;

namespace SQLWorker.AcceptanceTests
{
    public class EndToEnd : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        
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
        
        [Fact]
        public async Task ScriptLaunchPingTest_ReturnOk()
        {
            var client = _factory.CreateClient();

            var launch = new LaunchInfoDTO
            {
                PathToDirectory = @"E:\",
                Parameters = new List<ParamInfoDTO>
                {
                    new ParamInfoDTO
                    {
                        Name = "_id",
                        Value = "1"
                    }
                },
                FileType = "csv"
            };

            var response = await client.PostAsJsonAsync("/Script/Launch", launch);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }

        [Fact]
        public async Task GetFileTree_ReturnOk()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsync("/Home/GetFileTree", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("dir", @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\")
            }));
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
            response.Should().NotBeNull();
        }


        [Fact]
        public async Task GetParams_ReturnOk()
        {
            var client = _factory.CreateClient();
            var response =
                await client.GetAsync(
                    @"/Script/GetParams?src=E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\testScript.sql");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }
    }
}