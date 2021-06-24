using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCMS.Utilities.Senders
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isHtml);
    }
}
