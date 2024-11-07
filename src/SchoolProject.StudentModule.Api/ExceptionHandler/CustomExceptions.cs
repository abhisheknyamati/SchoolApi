namespace SchoolProject.StudentModule.API.ExceptionHandler
{
    public class StudentNotFound : Exception
    {
        public StudentNotFound(string message) : base(message)
        {
        }
    }

    public class EmailAlreadyRegistered : Exception
    {
        public EmailAlreadyRegistered(string message) : base(message)
        {
        }
    }
}