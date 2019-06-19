using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Services
{
  public interface IEmailSender
  {
    Task SendEmailAsync(string email, string subject, string message);
    string SendPlainEmailAsync(string emailAdd, string subject, string message);

    string SendLinkEmailAsync(string emailAdd, string subject, string message);

    Task SendGridEmailAsync(string emailAdd, string subject, string message);

    Task GmailSendEmail(string email, string CallbackUrl, string role);
  }
}
