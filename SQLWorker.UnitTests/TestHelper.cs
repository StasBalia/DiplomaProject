using System.Collections.Generic;
using System.Data;
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


        public static DataSet SimpleDataSet()
        {
            DataSet ds = new DataSet
            {
                Tables =
                {
                    new DataTable
                    {
                        Columns =
                        {
                            new DataColumn
                            {
                                ColumnName = "colName",
                                DataType = typeof(int)
                            },
                            new DataColumn
                            {
                                ColumnName = "col1Name",
                                DataType = typeof(int)
                            }
                        }
                        
                    }
                }
            };
            var table = ds.Tables[0];
            var dr = table.NewRow();
            dr.ItemArray = new object[] {1, 2};
            table.Rows.Add(dr);
            var dr2 = table.NewRow();
            dr2.ItemArray = new object[] {3, 4};
            table.Rows.Add(dr2);
            var dr3 = table.NewRow();
            dr3.ItemArray = new object[] {5, 6};
            table.Rows.Add(dr3);
            return ds;
        }
    }
}