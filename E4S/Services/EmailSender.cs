using E4S.Helpers;
using E4S.Models;
using E4S.Models.AccountInventory;
using E4S.Models.HumanResource;
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
    string apiKey = "";

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
      SmtpServer.Credentials = new System.Net.NetworkCredential("spoc@wragbysolutions.com", "@$Wbst@m!n18");

      //SmtpServer.Credentials = new System.Net.NetworkCredential("Wragbydev@wragbysolutions.com", "@Devops19");
      try
      {
        MailMessage mailMessage = new MailMessage();
        MailMessage mail = mailMessage;
        //mail.From = new MailAddress("Wragbydev@wragbysolutions.com");
        mail.From = new MailAddress("spoc@wragbysolutions.com");
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

    public string SendLinkEmailAsync(string emailAdd, string subject, string message)
    {
      SmtpServer.Credentials = new System.Net.NetworkCredential("spoc@wragbysolutions.com", "@$Wbst@m!n18");

      //SmtpServer.Credentials = new System.Net.NetworkCredential("Wragbydev@wragbysolutions.com", "@Devops19");
      try
      {
        MailMessage mailMessage = new MailMessage();
        MailMessage mail = mailMessage;
        //mail.From = new MailAddress("Wragbydev@wragbysolutions.com");
        mail.From = new MailAddress("spoc@wragbysolutions.com");
        mail.To.Add(emailAdd);
        mail.Subject = subject;
        mail.Body = message;
        mail.IsBodyHtml = true;
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


    async Task IEmailSender.SendGridEmailAsync(string emailAdd, string subject, string message, string organisation, string firstname, string template)
    {

      string domain = "http://erp4sme.azurewebsites.net";

      EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

      var body = EmailHelper.GetTemplate(template).Replace("#FirstName", firstname).Replace("#ResetLink", message).Replace("#HostDomain", domain).Replace("#Organisation", organisation);

      //string 

      var client = new SendGridClient(apiKey);
      var msg = new SendGridMessage()
      {
        From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
        Subject = "Set Up Your Account on ERP4SME.",
        //PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
        HtmlContent = body
        
        
      };
      msg.AddTo(new EmailAddress(emailAdd, emailAdd));
      await client.SendEmailAsync(msg);
      //var response = 

    }


    async Task IEmailSender.SendGridEmailConfrimationAsync(string emailAdd, string subject, string message, string firstname)
    {

      string domain = "http://erp4sme.azurewebsites.net";
      
      EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

      var body = EmailHelper.GetTemplate("accoutConfirmation").Replace("#FirstName", firstname).Replace("#ResetLink", message).Replace("#HostDomain", domain);

      //string 

      var client = new SendGridClient(apiKey);
      var msg = new SendGridMessage()
      {
        From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
        Subject = "Account Confirmationon ERP4SME.",
        //PlainTextContent = "You have are registered on ERP4SME platform. Kindly confrim your account.",
        HtmlContent = body


      };
      msg.AddTo(new EmailAddress(emailAdd, emailAdd));
      await client.SendEmailAsync(msg);
      //var response =

    }

    async Task IEmailSender.GmailSendEmail(string email, string CallbackUrl, string role)
    {
      SmtpClient client = new SmtpClient("smtp.gmail.com"); //set client 
      client.Port = 587;
      client.EnableSsl = true;
      client.DeliveryMethod = SmtpDeliveryMethod.Network;
      client.UseDefaultCredentials = false;
      client.Credentials = new System.Net.NetworkCredential("lawaledamilare@gmail.com", "92rapper"); //Mailing credential
                                                                                                     //mail body
      MailMessage mailMessage = new MailMessage();
      mailMessage.From = new MailAddress("no-reply@erp4sme.com", "ERP4SME");
      //mailMessage.CC.Add(new MailAddress("anenetemocu@gmail.com", "CC Name"));
      mailMessage.Bcc.Add(new MailAddress("junex07@hotmail.com", "BCC Name"));
      mailMessage.To.Add(email); //swap with verifyID.ComppanyEmail on go live
      mailMessage.Body = "Your Email Account (" + email + ") has been used for a password recovery attempt, on ERP4SME platform at " + DateTime.Now + ". Please confirm your account by clicking " + "<strong><a href='" + CallbackUrl + "'>here</a></strong> to set-up your account.";
      mailMessage.Sender = new MailAddress("no-reply@erp4sme.com");
      mailMessage.Subject = "CHANGE YOUR PASSWORD";
      mailMessage.IsBodyHtml = true;

      //mailMessage.
      client.Send(mailMessage);
    }

    public async Task SendGridLeaveRequestAsync(string emailAdd, string subject, string message, string firstname, string template, string organisation, Leave leave)
    {

      string domain = "http://erp4sme.azurewebsites.net";

      EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

      var body = EmailHelper.GetTemplate(template).Replace("#FirstName", firstname).Replace("#ResetLink", domain+message).Replace("#HostDomain", domain).Replace("#Organisation", organisation).Replace("#EmployeeFirstname", leave.EmployeeDetail.FirstName + " " + leave.EmployeeDetail.LastName).Replace("#LeaveTitle", leave.LeaveTitle).Replace("#StartDate", leave.StartDate.ToString("dd, MMM, yy")).Replace(" #CalculatedDays", leave.CalculatedDays.ToString());

      //string 

      var client = new SendGridClient(apiKey);
      var msg = new SendGridMessage()
      {
        From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
        Subject = "New Leave Request on ERP4SME.",
        //PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
        HtmlContent = body


      };
      msg.AddTo(new EmailAddress(emailAdd, emailAdd));
      await client.SendEmailAsync(msg);
      //var response = 
    }


    public async Task SendGridLeaveApprovalAsync(string emailAdd, string subject, string message, string firstname, string template, string organisation, Leave leave)
    {

        string domain = "http://erp4sme.azurewebsites.net";

        EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

        var body = EmailHelper.GetTemplate(template).Replace("#FirstName", firstname).Replace("#ResetLink", domain + message).Replace("#HostDomain", domain).Replace("#Organisation", organisation).Replace("#EmployeeFirstname", leave.EmployeeDetail.FirstName + " " + leave.EmployeeDetail.LastName).Replace("#LeaveTitle", leave.LeaveTitle).Replace("#StartDate", leave.StartDate.ToString("dd MMM, yy")).Replace("#EndDate", leave.EndDate.ToString("dd MMM, yy"));
           

        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
            Subject = "New Leave Approval on ERP4SME.",
            //PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
            HtmlContent = body


        };
        msg.AddTo(new EmailAddress(emailAdd, emailAdd));
        await client.SendEmailAsync(msg);
        //var response = 
    }

        public async Task SendGridLeaveDeclinedAsync(string emailAdd, string subject, string message, string firstname, string template, string organisation, Leave leave)
        {

            string domain = "http://erp4sme.azurewebsites.net";

            EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

            var body = EmailHelper.GetTemplate(template).Replace("#FirstName", firstname).Replace("#ResetLink", domain + message).Replace("#HostDomain", domain).Replace("#Organisation", organisation).Replace("#EmployeeFirstname", leave.EmployeeDetail.FirstName + " " + leave.EmployeeDetail.LastName).Replace("#LeaveTitle", leave.LeaveTitle).Replace("#StartDate", leave.StartDate.ToString("dd MMM, yyyy")).Replace("#EndDate", leave.EndDate.ToString("dd MMM, yyyy"));
       

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
                Subject = "New Leave Declined on ERP4SME.",
                //PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
                HtmlContent = body


            };
            msg.AddTo(new EmailAddress(emailAdd, emailAdd));
            await client.SendEmailAsync(msg);
            //var response = 
        }

        public async Task SendGridLeaveApplicationAsync(string emailAdd, string subject, string message, string firstname, string template, string organisation, Leave leave)
        {

            string domain = "http://erp4sme.azurewebsites.net";

            EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

            var body = EmailHelper.GetTemplate(template).Replace("#FirstName", firstname).Replace("#HostDomain", domain).Replace("#Organisation", organisation).Replace("#EmployeeFirstname", leave.EmployeeDetail.FirstName + " " + leave.EmployeeDetail.LastName).Replace("#LeaveTitle", leave.LeaveTitle).Replace("#StartDate", leave.StartDate.ToString("dd MMM, yyyy")).Replace("#EndDate", leave.EndDate.ToString("dd MMM, yyyy"));

            //string apiKey 

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
                Subject = "New Leave Application on ERP4SME.",
                //PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
                HtmlContent = body


            };
            msg.AddTo(new EmailAddress(emailAdd, emailAdd));
            await client.SendEmailAsync(msg);
            //var response = 
        }




        //Email Templates for Account Module,

        public async Task SendGridInvoicesAsync(string emailAdd, string subject, string message, string firstname, string template, string organisation, InvoiceRecord invoiceRecord, Customer customer)
        {

            string domain = "http://erp4sme.azurewebsites.net";

            EmailTemplateHelper EmailHelper = new EmailTemplateHelper();

            var body = EmailHelper.GetTemplate(template).Replace("#FirstName", firstname).Replace("#HostDomain", domain).Replace("#Organisation", organisation).Replace("#CustomersFirstname", customer.CustomerName ).Replace("#DueDate", invoiceRecord.DueDate.ToString("dd MMM, yyyy")).Replace("#Amount", invoiceRecord.Total.ToString());

            //string apiKey 

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@erp4sme.ng", "ERP4SME"),
                Subject = "New Invoice on ERP4SME.",
                //PlainTextContent = "You have been registered on ERP4SME platform. Kindly use the link below to create your account.",
                HtmlContent = body


            };
            msg.AddTo(new EmailAddress(emailAdd, emailAdd));
            await client.SendEmailAsync(msg);
            //var response = 
        }


    }
}
