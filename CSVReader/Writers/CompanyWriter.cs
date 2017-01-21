using Common.Model;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using CSVReader.FileInterface;

namespace CSVReader.Writers
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
            TempFileWorker.CreateTempDirectory();

            var filename = Path.GetFileNameWithoutExtension(OutputFile);
            var startupsFile = FileSufix.CreateSufix(filename, SufixType.Startups);
            var dealflowFile = FileSufix.CreateSufix(filename, SufixType.Dealflow);

            using (var startupsFileWriter = GetWriter(startupsFile))
            using (var dealflowFileWriter = GetWriter(dealflowFile))
            {
                foreach (var company in companies)
                {
                    startupsFileWriter.WriteLine(company.Serialize());
                    if (company.DealFlow != Common.DealFlowType.None)
                    {
                        dealflowFileWriter.WriteLine(company.GetDealFlowString());
                    }
                }
            }

            CreateZipArchive();
            TempFileWorker.DeleteTempDir();
        }

        private void CreateZipArchive()
        {
            var zipName = FileSufix.CreateSufix(OutputFile, SufixType.Zip);
            FileExtention.DeleteFileIfExists(zipName);
            ZipFile.CreateFromDirectory(TempFileWorker.TempDir, zipName);
        }

        private StreamWriter GetWriter(string file)
        {
            string fileName = Path.GetFileName(file);
            string filePath = Path.Combine(TempFileWorker.TempDir, fileName);
            return new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate));
        }
    }
}
