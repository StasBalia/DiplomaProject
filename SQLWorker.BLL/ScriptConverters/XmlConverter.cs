using System.Data;
using System.IO;
using SQLWorker.BLL.Models.Interfaces;

namespace SQLWorker.BLL.ScriptConverters
{
    public class XmlConverter : IScriptConverter<string>
    {
        public string ConvertToRightFormat(DataSet result)
        {
            StringWriter writer = new StringWriter();
            result.Tables[0].WriteXml(writer, XmlWriteMode.WriteSchema, false);
            return writer.ToString();
        }
    }
}