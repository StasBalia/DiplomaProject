using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SQLWorker.BLL.Models.Enums;
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
        [InlineData(FileExtension.csv, typeof(FileStreamResult))]
        public async Task ScriptWorker_CorrectFileExtension_ReturnCorrectResult(FileExtension fileExtension,
            Type returnType)
        {
            var result = await _controller.ConvertResultToActionResultAsync(new DownloadInfoDTO
            {
                FileName = "fileScript.sql_24.2.2019_332759",
                FileType = "csv",
                SavedPath = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\fileScript.sql_24.2.2019_332759.csv"
            }, fileExtension);
            Type resultType = result.GetType();
            resultType.IsAssignableFrom(returnType).Should().BeTrue();
        }
    }
}