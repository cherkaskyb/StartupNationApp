using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader.DataReaders
{
    public class DealFlowReader
    {
        private char[] delimiter = new char[] { '\t' };
        private Type TypeOfDealFlow = typeof(DealFlowType);
        public DealFlowReader(string inputFile)
        {
            InputFile = inputFile;
        }

        public string InputFile { get; private set; }

        public IDictionary<string, DealFlowType> ReadDealFlows()
        {
            var results = new Dictionary<string, DealFlowType>();
            using (var dealFlowStream = new StreamReader(new FileStream(InputFile, FileMode.Open)))
            {
                var line = dealFlowStream.ReadLine();
                while (line != null)
                {
                    var parts = line.Split(delimiter);
                    results.Add(parts[0], (DealFlowType)Enum.Parse(TypeOfDealFlow, parts[1]));
                    line = dealFlowStream.ReadLine();
                }
            }

            return results;
        }
    }
}
