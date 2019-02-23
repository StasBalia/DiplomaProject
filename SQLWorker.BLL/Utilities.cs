using System;

namespace SQLWorker.BLL
{
    public class Utilities
    {
        public static string GenerateFileNameForResult(string scriptNameWithoutFileExtension) =>
            $"{scriptNameWithoutFileExtension}_{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}_{new Random().Next(1, 999999).ToString().PadLeft(6, '0')}";
    }
}