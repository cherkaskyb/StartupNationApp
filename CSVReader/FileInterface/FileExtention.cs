using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader.FileInterface
{
    public static class FileExtention
    {
        public static void DeleteDirectoryIfExists(string dirpath)
        {
            if (Directory.Exists(dirpath))
            {
                Directory.Delete(dirpath, true);
            }
        }

        public static void DeleteFileIfExists(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}
