using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SQLWorker.BLL;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.ScriptUtilities;
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
            ScriptSources.Add(new ScriptInfo
            {
                Name = "testScript.sql",
                Parameters = new List<string> {"{id}"},
                Path = @"Scripts\github\testScript.sql",
                Provider = "github"
            });
            var launch = new LaunchInfoDTO
            {
                PathToDirectory = @"Scripts\github\testScript.sql",
                Parameters = "[{\"Name\":\"{id}\",\"Value\":\"2\"}]",
                FileType = "csv"
            };

            var response = await client.PostAsJsonAsync("/Script/Launch", launch);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
            ScriptSources.RemoveAll();
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
                    @"/Script/GetParams?path=E:\University\Diploma\DiplomaProject\SQLWorker.AcceptanceTests\bin\Debug\netcoreapp2.2\Scripts\github\testScript.sql");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }

        [Fact]
        public async Task Source_CorrectPath_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var response =
                await client.GetAsync(
                    @"Script/Source?src=Scripts\github\testScript.sql");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }

        [Fact]
        public async Task DownloadCorrectInfoForDownload_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var response =
                await client.GetAsync(
                    @"Script/Download?fileType=csv&fileName=testScript.sql_24.2.2019_042565.csv&savedPath=Results\github_Results\testScript.sql_24.2.2019_042565.csv");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }

        [Fact]
        public async Task GetTasksForUser_ReturnsOk() //TODO: It works for all requests now. In future, I will edit it for concrete user.
        {
            var client = _factory.CreateClient();
            var response =
                await client.GetAsync(
                    @"Script/GetTasksForUser");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }

        [Fact]
        public async Task GetScriptInfo_ReturnsOk()
        {
            TaskModel model = new TaskModel
            {
                Id = Guid.NewGuid()
            };
            TaskHandler.AddTask(model);
            
            var client = _factory.CreateClient();
            var response =
                await client.GetAsync(
                    $@"Script/GetScriptInfo?guid={model.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.IsSuccessStatusCode.Should().Be(true);
        }
        
    }
}