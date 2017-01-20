using Common.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testing
{
    public class CompanySirializationTests
    {
        [Fact]
        public void SirializeEmptyCompany_EmptyLine()
        {
            var c = new Company();
            var result = c.Serialize();
            Assert.Equal("\t1/1/0001 00:00:00\t1/1/0001 00:00:00\tA\t0\t\t", result);
        }

        [Fact]
        public void SirializeEmptyCompany_FullValuesCompany()
        {
            var c = new Company()
            {
                AmountRaisedInMilUsd = 5.5f,
                Established = new DateTime(2006, 11, 2),
                LastFunding = new DateTime(2016, 6, 3),
                LinkToFinder = @"https://finder.startupnationcentral.org/c/countrz",
                LinkToHomepage = @"https://finder.startupnationcentral.org",
                Name = "CompanyName",
                Stage = Common.CompanyStage.A
            };

            var result = c.Serialize();
            Assert.Equal(@"CompanyName\t11/2/2006 00:00:00\t6/3/2016 00:00:00\tA\t5.5\thttps://finder.startupnationcentral.org/c/countrz\thttps://finder.startupnationcentral.org", result);
        }
    }
}
