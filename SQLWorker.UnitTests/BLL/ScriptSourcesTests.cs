using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SQLWorker.BLL;
using Xunit;

namespace SQLWorker.UnitTests.BLL
{
    public class ScriptSourcesTests
    { 
        [Fact]
        public void AddSingleElement_ReturnsOneAsCountOfAll()
        {
            ScriptSources.Add(new ScriptInfo());
            ScriptSources.GetAll().Count().Should().Be(1);
        }
        
        [Theory]
        [InlineData(0,0)]
        [InlineData(3,3)]
        public void GetAll_ReturnsCurrentCount(int forI, int expectedCount)
        {
            for(int i = 0; i < forI; i++)
                ScriptSources.Add(new ScriptInfo());
            var t = ScriptSources.GetAll();
            ScriptSources.GetAll().Count().Should().Be(expectedCount);
            ScriptSources.RemoveAll();
        }

        [Fact]
        public void RemoveAll_ReturnsZeroAsCurrentCount()
        {
            ScriptSources.RemoveAll();
            ScriptSources.GetAll().Count().Should().Be(0);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(2,2)]
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
    }
}