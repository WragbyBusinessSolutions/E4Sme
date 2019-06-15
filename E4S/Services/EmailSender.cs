using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace E4S.Services
{
  // This class is used by the application to send email for account confirmation and password reset.
  // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
  public class EmailSender : IEmailSender
  {
    SmtpClient SmtpServer;
    string MailerResponse;
    public EmailSender()
    {

      SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
      SmtpServer = smtpClient;
      SmtpServer.Port = 587;
      SmtpServer.EnableSsl = true;
      SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
      MailerResponse = "";




    }

    public Task SendEmailAsync(string email, string subject, string message)
    {
      return Task.CompletedTask;
    }

    //public async Task<string> SendGridEmailAsync(string emailAdd, string subject, string message)
    //{
    //}

    public string SendPlainEmailAsync(string emailAdd, string subject, string message)
    {
      SmtpServer.Credentials = new System.Net.NetworkCredential("Wragbydev@wragbysolutions.com", "@Devops19");
      try
      {
        MailMessage mailMessage = new MailMessage();
        MailMessage mail = mailMessage;
        mail.From = new MailAddress("Wragbydev@wragbysolutions.com");
        mail.To.Add(emailAdd);
        mail.Subject = subject;
        mail.Body = message;
        SmtpServer.Send(mail);
        MailerResponse = "Success";
      }
      catch (Exception ex)
      {
        MailerResponse = "Failure";
        var ErrorMessage = ex.Message;
      }
      return MailerResponse;
    }

    async Task IEmailSender.SendGridEmailAsync(string emailAdd, string subject, string message)
    {
      var apiKey = "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";

      //new code
      //var apiKey = "SG.TntgdS0USueQiV3A - dnAQQ.L6ty2Nh - N75ZniKaxRGUpPUc4TJSeKDCtaoANyCOJhU";
      var client = new SendGridClient(apiKey);
      var msg = new SendGridMessage()
      {
        From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
        Subject = "Set Up Your Account on ERP4SME.",
        PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
        HtmlContent = "<strong> " + message + "</strong>",
      };
      msg.AddTo(new EmailAddress(emailAdd, emailAdd));
      var response = await client.SendEmailAsync(msg);

    }
  }
}
