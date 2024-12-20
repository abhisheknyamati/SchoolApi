namespace SchoolProject.StudentModule.API.ExceptionHandler
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