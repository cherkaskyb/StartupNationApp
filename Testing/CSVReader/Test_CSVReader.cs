using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVReader;
using System.IO;
using System.Reflection;

namespace Testing.CSVDataReader
{
    public class Test_CSVReader
    {
        public void TestInitialize()
        {
            var path = GetPathToInputFile();
            //_reader = new CSV2WebQueriesDataRetriver();
        }

        private static string GetPathToInputFile()
        {
            var currentDir = Path.GetFullPath(Directory.GetCurrentDirectory());
            var baseDir = Path.Combine(Path.Combine(currentDir, ".."), "..");
            var fileDir = Path.Combine(Path.Combine(baseDir, "CSVReader"), "InputFiles");
            return Path.Combine(fileDir, "company_export.csv");
        }
    }
}
