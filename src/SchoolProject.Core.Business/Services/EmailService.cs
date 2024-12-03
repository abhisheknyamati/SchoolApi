
using System.Net;
using System.Net.Mail;

namespace SchoolProject.Core.Business.Services.Interfaces
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string toEmail, string subject, string message)
        {
            var fromEmail = "itkarsanskruti53@gmail.com";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("itkarsanskruti53@gmail.com", "ifba emng qlbn fvrr"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, message);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}