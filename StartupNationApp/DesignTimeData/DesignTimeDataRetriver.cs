using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using System.Threading;

namespace StartupNationApp.DesignTimeData
{
    public class DesignTimeDataRetriver : IDataRetriver
    {
        public EventHandler<Company> CompanyAdded
        {
            get; set;
        }

        public Task StartGetCompanies()
        {
            var t = new Timer(state => RaiseCompanyAdded(), null, 0, 1000);
            Thread.Sleep(15000);
            return new TaskCompletionSource<int>().Task;
        }

        private void RaiseCompanyAdded()
        {
            var random = new Random();
            var company = new Company()
            {
                AmountRaisedInMilUsd = (Single)new Random().NextDouble(),
                Established = DateTime.Now,
                LinkToFinder = "asdfasdf",
                LinkToHomepage = "asdfasdf",
                Name = "Name",
                Stage = Common.CompanyStage.Bootstrapped
            };

            var handler = CompanyAdded;
            if (handler != null)
            {
                handler.Invoke(this, company);
            }
        }

        public void StopGetCompanies()
        {
        }

        public void Init(string path)
        {
        }

        public void ContinueWith(ICollection<Company> existingCompanies)
        {
            throw new NotImplementedException();
        }

        public Task StartGetCompanies(ICollection<Company> existingCompanies)
        {
            throw new NotImplementedException();
        }
    }
}
