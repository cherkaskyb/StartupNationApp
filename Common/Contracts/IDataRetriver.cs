using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IDataRetriver
    {
        EventHandler<Company> CompanyAdded { get; set; }
        Task StartGetCompanies(ICollection<Company> existingCompanies);
        void StopGetCompanies();
        void Init(string path);
    }
}
