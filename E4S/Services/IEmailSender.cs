using E4S.Models.HumanResource;
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

    Task SendGridEmailAsync(string emailAdd, string subject, string message, string organisation, string firstname, string template);

    Task GmailSendEmail(string email, string CallbackUrl, string role);
    Task SendGridEmailConfrimationAsync(string emailAdd, string subject, string message, string firstname);
    Task SendGridLeaveRequestAsync(string emailAdd, string subject, string message, string firstname, string template, string organisation, Leave leave);
    Task SendGridLeaveApprovalAsync(string email, string subject, string message, string firstname, string template, string organisation, Leave leave);
    Task SendGridLeaveDeclinedAsync(string email, string subject, string message, string firstname, string template, string organisation, Leave leave);
    Task SendGridLeaveApplicationAsync(string email, string subject, string message, string firstname, string template, string organisation, Leave leave);

    }
}

