using Common.Contracts;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader
{
    public class DataRetriverFactory
    {
        private IMessageService _messageService;

        public DataRetriverFactory(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IDataRetriver GetRetriver(string file)
        {
            IDataRetriver retriver = null;
            if (FileSufix.Is(file, SufixType.Csv))
            {
                retriver = new CSVDataRetriver(_messageService);
            }

            if (FileSufix.Is(file, SufixType.Zip) || FileSufix.Is(file, SufixType.Startups))
            {
                retriver = new CompanyReader();
            }

            retriver.Init(file);
            return retriver;
        }
    }
}
