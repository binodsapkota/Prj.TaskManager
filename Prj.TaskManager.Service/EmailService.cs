



using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Prj.TaskManager.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        string smtpServer;
        int port;
        string senderEmail;
        string password;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            smtpServer = _configuration["EmailSettings:SmtpServer"];
            port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            senderEmail = _configuration["EmailSettings:SenderEmail"];
            password = _configuration["EmailSettings:SenderPassword"];
        }

        //public async Task SendEmailAsync(string toEmail, string subject, string body)
        //{

        //    var message = new MimeMessage();
        //    var fromMailBoxAddress = new MailboxAddress("Task Manager", senderEmail);
        //    message.From.Add(fromMailBoxAddress);
        //    var toMailboxAddress = new MailboxAddress("", toEmail);
        //    message.To.Add(toMailboxAddress);
        //    message.Subject = subject;
        //    var bodyBuilder = new BodyBuilder { HtmlBody = body };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync(smtpServer, port, false);
        //        await client.AuthenticateAsync(senderEmail, password);
        //        await client.SendAsync(message);
        //        await client.DisconnectAsync(true);
        //    }

        //}

        public async Task SendEmail2Async(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(
                    senderEmail,
                    password
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(to),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
