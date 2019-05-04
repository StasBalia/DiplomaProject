using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SQLWorker.BLL.Models;
using SQLWorker.BLL.ScriptUtilities;
using Xunit;

namespace SQLWorker.UnitTests.BLL.ScriptsT
{
    public class ScriptSourcesTests
    {
        private readonly object _objLock = new object();
        
        [Fact]
        public void AddSingleElement_ReturnsOneAsCountOfAll()
        {
                ScriptSources.Add(new ScriptInfo());
                ScriptSources.GetAll().Count().Should().Be(1);
                ScriptSources.RemoveAll();
        }
        
        [Theory]
        [InlineData(0,0)]
        [InlineData(3,3)]
        public void GetAll_ReturnsCurrentCount(int forI, int expectedCount)
        {
                for (int i = 0; i < forI; i++)
                    ScriptSources.Add(new ScriptInfo());
                ScriptSources.GetAll().Count().Should().Be(expectedCount);
                ScriptSources.RemoveAll();
        }

        [Fact]
        public void RemoveAll_ReturnsZeroAsCurrentCount()
        {
                ScriptSources.Add(new ScriptInfo());
                ScriptSources.RemoveAll();
                ScriptSources.GetAll().Count().Should().Be(0);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(3,3)]
        public void AddRange_ReturnsCurrentCount(int forI, int expectedCount)
        {
                List<ScriptInfo> list = new List<ScriptInfo>();
                for (int i = 0; i < forI; i++)
                    list.Add(new ScriptInfo());
                ScriptSources.AddRange(list);
                ScriptSources.GetAll().Count().Should().Be(expectedCount);
                ScriptSources.RemoveAll();
        }

        [Fact]
        public void GetScriptByFilePath_ReturnsFile()
        {
            var expected = new ScriptInfo
            {
                Path = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\testScript.sql"
            };
            ScriptSources.Add(expected);
            var filePath = @"E:\University\Diploma\DiplomaProject\SQLWorker.Web\Scripts\github\testScript.sql";

            ScriptInfo result = ScriptSources.GetSingleScriptByFilePath(filePath);
            ScriptSources.RemoveAll();
            result.Should().BeEquivalentTo(expected);
        }
    }
}