using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVReader;
using System.IO;
using System.Reflection;

namespace Testing.CSVDataReader
{
    [TestClass]
    public class Test_CSVReader
    {
        private CSVDataRetriver _reader;

        [TestInitialize]
        public void TestInitialize()
        {
            var path = GetPathToInputFile();
            //_reader = new CSVDataRetriver();
        }

        private static string GetPathToInputFile()
        {
            var currentDir = Path.GetFullPath(Directory.GetCurrentDirectory());
            var baseDir = Path.Combine(Path.Combine(currentDir, ".."), "..");
            var fileDir = Path.Combine(Path.Combine(baseDir, "CSVReader"), "InputFiles");
            return Path.Combine(fileDir, "company_export.csv");
        }

        [TestMethod]
        public void ReadSingleCompany()
        {
        }
    }
}
