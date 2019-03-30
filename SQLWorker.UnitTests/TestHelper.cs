using System.Collections.Generic;
using ClosedXML.Excel;

namespace SQLWorker.UnitTests
{
    public class TestHelper
    {
        public static List<string> ExtractSpecificDataFromWorksheet(IXLWorksheet worksheet)
        {
            List<string> list = new List<string>();
            for (int i = 65; i < 67; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    list.Add((char)i + j.ToString());
                }    
            }

            return list;
        }
    }
}