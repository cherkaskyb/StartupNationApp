using Common.Utils;
using CSVReader.FileInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader.FileInterface
{
    public class TempFileWorker
    {
        public const string TempDir = "Temp";

        public static void CreateTempDirectory()
        {
            DeleteTempDir();
            Directory.CreateDirectory(TempDir);
        }

        internal static void DeleteTempDir()
        {
            FileExtention.DeleteDirectoryIfExists(TempDir);
        }

        internal static string GetFilePathInTempDirectory(string originalFilename, SufixType sufix)
        {
            var file = FileSufix.CreateSufix(Path.GetFileNameWithoutExtension(originalFilename), sufix);
            return Path.Combine(TempDir, file);
        }
    }
}
