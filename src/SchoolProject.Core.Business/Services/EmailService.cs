using System.Net;
using System.Net.Mail;
using SchoolProject.Core.Business.Services.Interfaces;

namespace SchoolProject.Core.Business.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string toEmail, string subject, string message)
        {

            var fromEmail = "abhisheknyamati@gmail.com";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("abhisheknyamati@gmail.com", "zwwe ahoc dcjs vqjs"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, message);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}