using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using System.IO;

namespace CSVReader
{
    class CompanyReader : IDataRetriver
    {
        private string InputFile { get; set; }

        public EventHandler<Company> CompanyAdded { get; set; }

        public void Init(string path)
        {
            InputFile = path;
        }

        public Task StartGetCompanies(ICollection<Company> existingCompanies)
        {
            var hashed = HashSetConverter.FromCompanies(existingCompanies);
            return Task.Run(() => GetCompanies(hashed));
        }

        public void StopGetCompanies()
        {
            throw new NotImplementedException();
        }

        private void GetCompanies(HashSet<string> existringCompanies)
        {
            using (var stream = new StreamReader(new FileStream(InputFile, FileMode.Open)))
            {
                var line = stream.ReadLine();
                Company company;
                while (line != null)
                {
                    company = Company.Deserialize(line);
                    if (!existringCompanies.Contains(company.LinkToFinder))
                    {
                        RaiseCompanyAdded(company);
                    }
                    line = stream.ReadLine();
                }
            }
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
