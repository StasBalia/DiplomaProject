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
        [InlineData("fileScript_30.3.2019_049548.csv", @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\", FileExtension.csv, typeof(FileStreamResult))]
        [InlineData("Test.xml",@"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\", FileExtension.xml, typeof(ContentResult))]
        [InlineData("fileScript_30.3.2019_069278.xlsx", @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\", FileExtension.xlsx, typeof(FileStreamResult))]
        [InlineData("fileScriptXml.sql_30.3.2019_469639.json", @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Results\github_Results\", FileExtension.json, typeof(ContentResult))]
        [InlineData("", "", FileExtension.json, typeof(EmptyResult))]
        [InlineData("fileScriptXml.sql_30.3.2019_469639.json", "", (FileExtension)1, typeof(EmptyResult))]
        public async Task ScriptWorker_CorrectFileExtension_ReturnCorrectResult(string fileName, string pathToSave, FileExtension fileExtension,
            Type returnType)
        {
            var result = await _controller.ConvertResultToActionResultAsync(new DownloadInfoDTO
            {
                FileName = fileName,
                FileType = fileExtension.ToString(),
                SavedPath = Path.Combine(pathToSave, fileName)
            }, fileExtension);
            Type resultType = result.GetType();
            resultType.IsAssignableFrom(returnType).Should().BeTrue();
        }
    }
}