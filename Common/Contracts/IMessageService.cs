using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IMessageService
    {
        EventHandler<string> NewMessage { get; set; }
        void Publish(string message);
    }
}
