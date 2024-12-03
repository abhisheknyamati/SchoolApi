namespace SchoolProject.Core.Business.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message);
    }
}