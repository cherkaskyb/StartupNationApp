using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using System.IO;
using System.IO.Compression;
using CSVReader.FileInterface;
using Common.Utils;
using Common;

namespace CSVReader.DataReaders
{
    class CompanyReader : IDataRetriver
    {
        private string InputFile { get; set; }
        private bool Stop { get; set; }

        public EventHandler<Company> CompanyAdded { get; set; }

        public void Init(string path)
        {
            InputFile = path;
            Stop = false;
        }

        public Task StartGetCompanies(ICollection<Company> existingCompanies)
        {
            var hashed = HashSetConverter.FromCompanies(existingCompanies);
            return Task.Run(() => GetCompanies(hashed));
        }

        public void StopGetCompanies()
        {
            Stop = true;
        }

        private void GetCompanies(HashSet<string> existringCompanies)
        {
            TempFileWorker.DeleteTempDir();

            IDictionary<string, DealFlowType> dealflows = new Dictionary<string, DealFlowType>();
            string startupsFile = string.Empty;
            if (FileSufix.Is(InputFile, SufixType.Zip))
            { 
                ZipFile.ExtractToDirectory(InputFile, TempFileWorker.TempDir);
                dealflows = GetDealFlows();
                startupsFile = TempFileWorker.GetFilePathInTempDirectory(InputFile, SufixType.Startups);
            } else
            {
                //Input file is startups file
                startupsFile = InputFile;
            }
            
            using (var startupsStream = new StreamReader(new FileStream(startupsFile, FileMode.Open)))
            {
                var line = startupsStream.ReadLine();
                Company company;
                while (line != null)
                {
                    if (Stop)
                    {
                        break;
                    }

                    company = Company.Deserialize(line);
                    if (dealflows.ContainsKey(company.Name))
                    {
                        company.DealFlow = dealflows[company.Name];
                    }

                    if (!existringCompanies.Contains(company.LinkToFinder))
                    {
                        RaiseCompanyAdded(company);
                    }
                    line = startupsStream.ReadLine();
                }
            }

            TempFileWorker.DeleteTempDir();
        }

        private IDictionary<string, DealFlowType> GetDealFlows()
        {
            var dealFlowReader = new DealFlowReader(
                TempFileWorker.GetFilePathInTempDirectory(InputFile, SufixType.Dealflow));
            return dealFlowReader.ReadDealFlows();
        }

        private void RaiseCompanyAdded(Company company)
        {
            var handler = CompanyAdded;
            if (handler != null)
            {
                handler.Invoke(this, company);
            }
        }
    }
}
