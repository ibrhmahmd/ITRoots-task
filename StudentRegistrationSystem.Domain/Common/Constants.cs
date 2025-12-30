namespace StudentRegistrationSystem.Domain.Common;


public static class Constants
{
    
    public static class Roles
    {
        public const string Student = "Student";
        public const string Admin = "Admin";
    }

    
    public static class RegistrationStatus
    {
        public const string Registered = "Registered";
        public const string Dropped = "Dropped";
        public const string Withdrawn = "Withdrawn";
        public const string Completed = "Completed";
    }

    
    public static class Semesters
    {
        public const string Fall = "Fall";
        public const string Spring = "Spring";
        public const string Summer = "Summer";
    }

    
    public static class Languages
    {
        public const string English = "en";
        public const string Arabic = "ar";
    }

    
    public static class SessionKeys
    {
        public const string UserId = "UserId";
        public const string Username = "Username";
        public const string Role = "Role";
        public const string IsEmailVerified = "IsEmailVerified";
    }

    
    public static class Cookies
    {
        public const string Language = "Language";
        public const string RememberMe = "RememberMe";
    }

    
    public static class Defaults
    {
        public const int PageSize = 10;
        public const int MaxPageSize = 100;
        public const string DefaultLanguage = "en";
    }

        
    public static class ErrorMessages
    {
        public const string NotFound = "The requested resource was not found.";
        public const string Unauthorized = "You are not authorized to perform this action.";
        public const string ValidationFailed = "Validation failed. Please check your input.";
        public const string DuplicateEntry = "A record with this information already exists.";
        public const string InvalidToken = "Invalid or expired token.";
        public const string SemesterStarted = "Cannot unregister: Semester has already started.";
        public const string CourseFull = "Course has reached maximum capacity.";
    }
}
