using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SQLWorker.BLL.Models.Enums;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.Web.Controllers;
using SQLWorker.Web.Models.Request.Script;
using Xunit;

namespace SQLWorker.UnitTests.Controller
{
    public class ScriptControllerTests
    {
        private readonly ScriptController _controller;

        public ScriptControllerTests()
        {
            var repository = new Mock<IScriptRepository>();
            _controller = new ScriptController(null,repository.Object);
        }

        [Theory]
        [InlineData("fileScript_30.3.2019_049548.csv", FileExtension.csv, typeof(FileStreamResult))]
        [InlineData("Test.xml", FileExtension.xml, typeof(ContentResult))]
        [InlineData("fileScript_30.3.2019_069278.xlsx", FileExtension.xlsx, typeof(FileStreamResult))]
        public async Task ScriptWorker_CorrectFileExtension_ReturnCorrectResult(string fileName, FileExtension fileExtension,
            Type returnType)
        {
            var result = await _controller.ConvertResultToActionResultAsync(new DownloadInfoDTO
            {
                FileName = fileName,
                FileType = fileExtension.ToString(),
                SavedPath = $@"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\{fileName}"
            }, fileExtension);
            Type resultType = result.GetType();
            resultType.IsAssignableFrom(returnType).Should().BeTrue();
        }
    }
}