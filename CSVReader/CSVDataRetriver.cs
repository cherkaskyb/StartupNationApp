using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using System.IO;
using System.Net;
using System.Xml.XPath;
using System.Xml;
using HtmlAgilityPack;
using System.Diagnostics;

namespace CSVReader
{
    public class CSVDataRetriver : IDataRetriver
    {
        #region Consts

        private const string finderSitePrefix = "finder.startupnationcentral.org";
        private const string homepagePrefix = @"http://";
        private const string htmlLocation = @"tmp.html";
        private static char[] delimiters = new[] { ',' };

        #endregion

        #region Fields

        private string InputFilePath { get; set; }
        private string[] DataLineInProcess { get; set; }
        private int CurrentIndexInLine { get; set; }
        private string CurrentCompanyHtml { get; set; }
        private string TempFilePath { get; set; }
        private HtmlDocument SiteHtml { get; set; }
        private bool Stop { get; set; }
        private IMessageService _messageService;

        #endregion

        public CSVDataRetriver(IMessageService messageService)
        {
            _messageService = messageService;
            CurrentIndexInLine = 0;
            TempFilePath = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), htmlLocation);
        }

        public void Init(string path)
        {
            InputFilePath = path;
        }

        public EventHandler<Company> CompanyAdded { get; set; }

        public Task StartGetCompanies(ICollection<Company> existingCompanies)
        {
            var hashed = HashSetConverter.FromCompanies(existingCompanies);
            return Task.Run(() =>
            {
                GetCompanies(hashed);
            });
        }

        public void StopGetCompanies()
        {
            Stop = true;
        }

        #region Get companies privates

        private IEnumerable<Company> GetCompanies(ICollection<string> existingCompanies)
        {
            List<Company> companies = new List<Company>();
            string line = string.Empty;
            string ContentLine = string.Empty;
            Stop = false;
            var checkInExistingCompanies = existingCompanies.Any();

            using (var stream = new StreamReader(new FileStream(InputFilePath, FileMode.Open)))
            {
                ReadHeaderLine(stream);

                while (true)
                {
                    if (Stop)
                    {
                        break;
                    }
                    line = stream.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    LineExplode(line);
                    var link = GetCompanyPathInFinder();
                    if (string.IsNullOrEmpty(link))
                    {
                        continue;
                    }

                    if (checkInExistingCompanies && existingCompanies.Contains(link))
                    {
                        continue;
                    }

                    var company = GetCompanyFromSite(link);
                    if (company != null)
                    {
                        RaiseCompanyAdded(company);
                        companies.Add(company);
                    }
                }
            }

            return companies;
        }

        private void RaiseCompanyAdded(Company company)
        {
            var handler = CompanyAdded;
            if (handler != null)
            {
                handler.Invoke(this, company);
            }
        }

        #endregion

        #region Html Reading helpers

        private Company GetCompanyFromSite(string link)
        {
            _messageService.Publish(string.Format("{0} Getting {1}", DateTime.Now, link));
            Stopwatch s = new Stopwatch();
            s.Start();
            var company = new Company();
            try
            {
                GetSiteHmtl(link);
            } catch (WebException e)
            {
                _messageService.Publish(string.Format("{0} Got WebException {1}", DateTime.Now, e.Message));
                return null;
            }
            SiteHtml = new HtmlDocument();
            SiteHtml.Load(TempFilePath);

            var value = GetFromHtml(XPathQueries.CompanyNameQuery);
            company.Name = value;
            company.LinkToFinder = link;

            var properties = SiteHtml.DocumentNode.SelectNodes(XPathQueries.CompanyProperties);
            if (properties == null)
            {
                _messageService.Publish(string.Format("{0} Null properties", DateTime.Now));
                return null;
            }
            string prop = string.Empty;
            foreach (var p in properties)
            {
                prop = p.SelectSingleNode(XPathQueries.PropertyHeaderInnerQuery).InnerText;
                value = p.SelectSingleNode(XPathQueries.PropertyValueInnerQuery).InnerText;

                company.FillProperty(prop, value);
            }

            properties = SiteHtml.DocumentNode.SelectNodes(XPathQueries.FundingRounds);
            HtmlNode valueNode;
            if (properties != null)
            {
                foreach (var p in properties)
                {
                    valueNode = p.SelectSingleNode(XPathQueries.FundingDateInnerQuery);
                    if (valueNode == null)
                    {
                        continue;
                    }
                    value = valueNode.InnerText;
                    company.SetLastFunding(value);
                }
            }
            
            _messageService.Publish(string.Format("{0} Getting {1} took {2}[sec]", DateTime.Now, link,
                TimeSpan.FromMilliseconds(s.ElapsedMilliseconds).TotalSeconds));
            return company;
        }

        private string GetFromHtml(string query)
        {
            return SiteHtml.DocumentNode.SelectSingleNode(query).InnerText;
        }

        private void GetSiteHmtl(string link)
        {
            WebRequest request = WebRequest.Create(string.Format("{0}{1}", homepagePrefix, link));
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }

            if (File.Exists(TempFilePath))
            {
                File.Delete(TempFilePath);
            }

            using (StreamWriter writer = new StreamWriter(TempFilePath))
            {
                writer.Write(html);
            }
        }

        #endregion

        #region Csv Reading helpers

        private void LineExplode(string line)
        {
            CurrentIndexInLine = 0;
            DataLineInProcess = line.Split(delimiters);
        }

        private string GetCompanyPathInFinder()
        {
            return SearchForPrefix(finderSitePrefix);
        }

        private string SearchForPrefix(string prefix)
        {
            while (CurrentIndexInLine < DataLineInProcess.Length)
            {
                if (DataLineInProcess[CurrentIndexInLine].StartsWith(prefix))
                {
                    return DataLineInProcess[CurrentIndexInLine];
                }

                CurrentIndexInLine++;
            }

            return string.Empty;
        }

        private void ReadHeaderLine(StreamReader stream)
        {
            stream.ReadLine();
        }

        #endregion

        internal static class XPathQueries
        {
            internal static string CompanyNameQuery = @"/html/body/div[2]/div/div/div/h1";
            internal static string CompanyProperties = @"/html/body/div[2]/div/div/div/ul/li";
            internal static string PropertyValueInnerQuery = @"div";
            internal static string PropertyHeaderInnerQuery = "strong";
            
            internal static string FundingRounds = @"/html/body/div[2]/div/div/div/div[5]/div/div";
            internal static string FundingDateInnerQuery = @"div/div/p[2]";
            ///html/body/div[2]/div/div/div/div[5]/div/div/div[1]/div/p[2]
        }
    }
}
