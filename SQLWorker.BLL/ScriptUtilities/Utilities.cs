using System;
using System.IO;
using System.Linq;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL.ScriptUtilities
{
    public class Utilities
    {
        public static string GenerateFileNameForResult(string scriptNameWithoutFileExtension) =>
            $"{scriptNameWithoutFileExtension}_{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}_{new Random().Next(1, 999999).ToString().PadLeft(6, '0')}.";

        public static FileExtension GetFileExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case "csv":
                    return FileExtension.csv;
                case "xml":
                    return FileExtension.xml;
                case "xlsx":
                    return FileExtension.xlsx;
                case "json":
                    return FileExtension.json;
                default:
                    return default(FileExtension);
            }
        }

        public static string GetFullPath(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2))
                return string.Empty;
            string path = Path.Combine(path1, path2);
            return new DirectoryInfo(path).FullName;
        }
    }
}