using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class FileSufix
    {
        public static string CreateSufix(string filepath, SufixType sufix)
        {
            var values = filepath.Split('.');
            if (values.Length > 1)
            {
                return filepath;
            }

            return $"{filepath}.{sufix}";
        }

        public static bool Is(string filename, SufixType sufix)
        {
            var words = filename.Split('.');
            return words.Last().ToLower() == sufix.ToString().ToLower();
        }
    }

    public enum SufixType
    {
        Startups, Dealflow,
        Zip,
        Csv
    }
}
