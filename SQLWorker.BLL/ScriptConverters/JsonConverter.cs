using System.Data;
using Newtonsoft.Json;
using SQLWorker.BLL.Models.Interfaces;

namespace SQLWorker.BLL.ScriptConverters
{
    public class JsonConverter : IScriptConverter<string>
    {
        public string ConvertToRightFormat(DataSet result)
        {
            return JsonConvert.SerializeObject(result);
        }
    }
}