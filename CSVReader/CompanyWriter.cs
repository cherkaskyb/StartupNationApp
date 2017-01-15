using Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader
{
    public class CompanyWriter
    {
        public string OutputFile { get; private set; }

        public CompanyWriter(string pathToOutputFile)
        {
            OutputFile = pathToOutputFile;
        }

        public void Write(IEnumerable<Company> companies)
        {
            if (File.Exists(OutputFile))
            {
                File.Delete(OutputFile);
            }
            using (var fileWriter = new StreamWriter(new FileStream(OutputFile, FileMode.OpenOrCreate)))
            {
                foreach (var company in companies)
                {
                    fileWriter.WriteLine(company.Serialize());
                }
            }
        }
    }
}
