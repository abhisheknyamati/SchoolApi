namespace SchoolProject.Core.Business.ExceptionHandler
{
    public class EmailAlreadyRegistered : Exception
    {
        public EmailAlreadyRegistered(string message) : base(message)
        {
        }
    }

    public class StudentAlreadyDeleted : Exception
    {
        public StudentAlreadyDeleted(string message) : base(message)
        {
        }
    }

}