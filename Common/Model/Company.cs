using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Company
    {
        #region Properties

        public string Name { get; set; }
        public DateTime Established { get; set; }
        public DateTime LastFunding { get; set; }
        public CompanyStage Stage { get; set; }
        public float AmountRaisedInMilUsd { get; set; }
        public string LinkToFinder { get; set; }
        public string LinkToHomepage { get; set; }
        public DealFlowType DealFlow { get; set; }

        #endregion

        public void FillProperty(string property, string value)
        {
            if (property == CompanyConsts.Properties.Homepage)
            {
                LinkToHomepage = value;
                return;
            }

            if (property == CompanyConsts.Properties.Stage)
            {
                FillStage(value);
            }

            if (property == CompanyConsts.Properties.AmountRaised)
            {
                AmountRaisedInMilUsd = DataParserHelper.ParseAmount(value);
            }

            if (property == CompanyConsts.Properties.Established)
            {
                Established = DataParserHelper.ParseDateTime(value);
            }
        }

        public string GetDealFlowString()
        {
            return $"{Name}\t{DealFlow}";
        }

        #region Filling privates

        private void FillStage(string value) 
        {
            if (value == CompanyConsts.Stage.A)
            {
                Stage = CompanyStage.A;
                return;
            }
            if (value == CompanyConsts.Stage.B)
            {
                Stage = CompanyStage.B;
                return;
            }
            if (value == CompanyConsts.Stage.Cp)
            {
                Stage = CompanyStage.Cplus;
                return;
            }
            if (value == CompanyConsts.Stage.Bootstrap)
            {
                Stage = CompanyStage.Bootstrapped;
                return;
            }
            if (value == CompanyConsts.Stage.Seed)
            {
                Stage = CompanyStage.Seed;
                return;
            }
        }

        #endregion

        #region Serialization

        public string Serialize()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}", 
                SerializationDelimiter, Name, Established, LastFunding, 
                Stage, AmountRaisedInMilUsd, LinkToFinder, LinkToHomepage);
        }

        private static char SerializationDelimiter = '\t';
        private static Type TypeOfCompanyStageEnum = typeof(CompanyStage);
        public static Company Deserialize(string company)
        {
            var values = company.Split(SerializationDelimiter);
            var result = new Company();
            result.Name = values[0];
            result.Established = DateTime.Parse(values[1]);
            result.LastFunding = DateTime.Parse(values[2]);
            result.Stage = (CompanyStage)Enum.Parse(TypeOfCompanyStageEnum, values[3]);
            result.AmountRaisedInMilUsd = Single.Parse(values[4]);
            result.LinkToFinder = values[5];
            result.LinkToHomepage = values[6];

            return result;
        }

        public void SetLastFunding(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            LastFunding = DateTime.Parse(value);
        }

        #endregion
    }

    internal static class CompanyConsts
    {
        internal static class Stage
        {
            internal const string A = "A";
            internal const string B = "B";
            internal const string Cp = "C+";
            internal const string Public = "Public";
            internal const string Seed = "Seed";
            internal const string Bootstrap = "Bootstrap";
        }

        internal static class Properties
        {
            internal const string Homepage = "Homepage";
            internal const string Stage = "Funding Stage";
            internal const string AmountRaised = "Amount Raised";
            internal const string Established = "Founded";
        }

    }
}
