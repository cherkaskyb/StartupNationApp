using Common.Contracts;
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
            var words = file.Split('.');
            IDataRetriver retriver = null;
            if (words.Last().ToLower() == "csv")
            {
                retriver = new CSVDataRetriver(_messageService);
            }

            if (words.Last().ToLower() == "startups")
            {
                retriver = new CompanyReader();
            }

            retriver.Init(file);
            return retriver;
        }
    }
}
