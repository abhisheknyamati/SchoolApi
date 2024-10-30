namespace SchoolApi.API.ExceptionHandler{
    public class ErrorMsgConstant
    {
        public const string StudentNotFound = "Student not found!!";
        public const string StudentAlreadyExists = "Student already exists";
        public const string StudentCreated = "Student created successfully";
        public const string StudentUpdated = "Student updated successfully";
        public const string StudentDeleted = "Student deleted successfully";
        public const string StudentNotDeleted = "Student not deleted";
        public const string StudentNotUpdated = "Student not updated";
        public const string StudentNotCreated = "Student not created";
        public const string StudentNotRetrieved = "Student not retrieved";
        public const string StudentRetrieved = "Student retrieved";
        public const string StudentListRetrieved = "Student list retrieved";
        public const string StudentListNotRetrieved = "Student list not retrieved";
        public const string StudentListEmpty = "Student list is empty";
        public const string PaginationPageSize = "Page size must be greater than 0 and less than or equal to 100.";
        public const string PaginationPageNumer = "Page number must be greater than 0.";
        public const string StudentAlreadyDeleted = "Couldn't delete student as it's is already deleted.";
    }
}