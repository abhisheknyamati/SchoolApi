namespace SchoolProject.StudentModule.API.ExceptionHandler
{
    public class StudentNotFound(string message) : Exception(message) // primary constructor
    {
    }

    public class EmailAlreadyRegistered : Exception
    {
        public EmailAlreadyRegistered(string message) : base(message)
        {
        }
    }
}