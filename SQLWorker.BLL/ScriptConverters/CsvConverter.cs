using System.Data;
using System.Linq;
using System.Text;
using SQLWorker.BLL.Models.Interfaces;

namespace SQLWorker.BLL.ScriptConverters
{
    public class CsvConverter : IScriptConverter<string>
    {
        public string ConvertToRightFormat(DataSet result)
        {
            DataTable table = result.Tables[0];          
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", result.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName)) + "\n");
            foreach (DataRow row in table.Rows)
            {
                sb.Append(string.Join(",", row.ItemArray) + "\n");
            }
            return sb.ToString();
        }
    }
}