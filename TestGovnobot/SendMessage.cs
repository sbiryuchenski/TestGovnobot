using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestGovnobot
{
    class SendMessage
    {
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public long ChatId { get; set; } = 0;
        public string Message { get; set; }

    }
}
