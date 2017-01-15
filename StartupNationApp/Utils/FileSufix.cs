using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupNationApp.Utils
{
    public static class FileSufix
    {
        public static string CreateSufix(string filepath)
        {
            var values = filepath.Split('.');
            if (values.Length > 1)
            {
                return filepath;
            }

            return string.Format("{0}.startups", filepath);
        }
    }
}
