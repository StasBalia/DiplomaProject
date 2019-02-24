using System;
using System.Linq;
using SQLWorker.BLL.Models.Enums;

namespace SQLWorker.BLL.ScriptUtilities
{
    public class Utilities
    {
        public static string GenerateFileNameForResult(string scriptNameWithoutFileExtension) =>
            $"{scriptNameWithoutFileExtension}_{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}_{new Random().Next(1, 999999).ToString().PadLeft(6, '0')}.";


        public static bool CheckIfFileExtensionExists(string fileExtension) =>
            Enum.GetNames(typeof(FileExtension)).Contains(fileExtension);

        public static FileExtension GetFileExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case "csv":
                    return FileExtension.csv;
                default:
                    return default(FileExtension);
            }
        }
    }
}