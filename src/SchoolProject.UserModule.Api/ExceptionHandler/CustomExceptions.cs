namespace SchoolProject.UserModule.Api.ExceptionHandler
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

    public class PageSizeException : Exception
    {
        public PageSizeException(string message) : base(message)
        {
        }
    }

    public class PageNumberException : Exception
    {
        public PageNumberException(string message) : base(message)
        {
        }
    }
}