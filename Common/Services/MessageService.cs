using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class MessageService : IMessageService
    {
        public EventHandler<string> NewMessage { get; set; }

        public void Publish(string message)
        {
            RaiseNewMessage(message);
        }

        private void RaiseNewMessage(string message)
        {
            var handler = NewMessage;
            if (handler != null)
            {
                handler.Invoke(this, message);
            }
        }
    }
}
