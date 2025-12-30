namespace StudentRegistrationSystem.Data.Queries;

/// <summary>
/// SQL queries for Course entity operations
/// </summary>
public static class CourseQueries
{
    public const string GetById = @"
        SELECT CourseId AS Id, CourseCode, CourseName, CourseNameAr, Description, 
               DescriptionAr, Credits, Semester, SemesterYear, SemesterStartDate, 
               MaxCapacity, IsActive, CreatedAt, UpdatedAt
        FROM Courses
        WHERE CourseId = @Id";

    public const string GetAll = @"
        SELECT CourseId AS Id, CourseCode, CourseName, CourseNameAr, Description, 
               DescriptionAr, Credits, Semester, SemesterYear, SemesterStartDate, 
               MaxCapacity, IsActive, CreatedAt, UpdatedAt
        FROM Courses
        ORDER BY CourseName";

    public const string GetAllActive = @"
        SELECT CourseId AS Id, CourseCode, CourseName, CourseNameAr, Description, 
               DescriptionAr, Credits, Semester, SemesterYear, SemesterStartDate, 
               MaxCapacity, IsActive, CreatedAt, UpdatedAt
        FROM Courses
        WHERE IsActive = 1
        ORDER BY CourseName";

    public const string GetBySemester = @"
        SELECT CourseId AS Id, CourseCode, CourseName, CourseNameAr, Description, 
               DescriptionAr, Credits, Semester, SemesterYear, SemesterStartDate, 
               MaxCapacity, IsActive, CreatedAt, UpdatedAt
        FROM Courses
        WHERE Semester = @Semester AND SemesterYear = @SemesterYear AND IsActive = 1
        ORDER BY CourseName";

    public const string Create = @"
        INSERT INTO Courses (CourseId, CourseCode, CourseName, CourseNameAr, Description, 
                           DescriptionAr, Credits, Semester, SemesterYear, SemesterStartDate, 
                           MaxCapacity, IsActive, CreatedAt, UpdatedAt)
        VALUES (@Id, @CourseCode, @CourseName, @CourseNameAr, @Description, 
                @DescriptionAr, @Credits, @Semester, @SemesterYear, @SemesterStartDate, 
                @MaxCapacity, @IsActive, @CreatedAt, @UpdatedAt)";

    public const string Update = @"
        UPDATE Courses
        SET CourseCode = @CourseCode,
            CourseName = @CourseName,
            CourseNameAr = @CourseNameAr,
            Description = @Description,
            DescriptionAr = @DescriptionAr,
            Credits = @Credits,
            Semester = @Semester,
            SemesterYear = @SemesterYear,
            SemesterStartDate = @SemesterStartDate,
            MaxCapacity = @MaxCapacity,
            IsActive = @IsActive,
            UpdatedAt = @UpdatedAt
        WHERE CourseId = @Id";

    public const string Delete = @"
        UPDATE Courses
        SET IsActive = 0, UpdatedAt = @UpdatedAt
        WHERE CourseId = @Id";

    public const string HasRegistrations = @"
        SELECT COUNT(1)
        FROM CourseRegistrations
        WHERE CourseId = @CourseId AND IsActive = 1";

    public const string CourseCodeExists = @"
        SELECT COUNT(1)
        FROM Courses
        WHERE CourseCode = @CourseCode AND (@ExcludeId IS NULL OR CourseId != @ExcludeId)";
}
