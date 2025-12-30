namespace StudentRegistrationSystem.Data.Queries;

public static class StudentQueries
{
    public const string GetById = @"
        SELECT StudentId AS Id, UserId, FullName, Phone, AcademicYear, 
               EnrollmentDate, CreatedAt, UpdatedAt
        FROM Students
        WHERE StudentId = @Id";

    public const string GetByUserId = @"
        SELECT StudentId AS Id, UserId, FullName, Phone, AcademicYear, 
               EnrollmentDate, CreatedAt, UpdatedAt
        FROM Students
        WHERE UserId = @UserId";

    public const string GetAll = @"
        SELECT StudentId AS Id, UserId, FullName, Phone, AcademicYear, 
               EnrollmentDate, CreatedAt, UpdatedAt
        FROM Students
        ORDER BY FullName";

    public const string Create = @"
        INSERT INTO Students (StudentId, UserId, FullName, Phone, AcademicYear, 
                             EnrollmentDate, CreatedAt, UpdatedAt)
        VALUES (@Id, @UserId, @FullName, @Phone, @AcademicYear, 
                @EnrollmentDate, @CreatedAt, @UpdatedAt)";

    public const string Update = @"
        UPDATE Students
        SET FullName = @FullName,
            Phone = @Phone,
            AcademicYear = @AcademicYear,
            EnrollmentDate = @EnrollmentDate,
            UpdatedAt = @UpdatedAt
        WHERE StudentId = @Id";

    public const string ExistsByUserId = @"
        SELECT COUNT(1)
        FROM Students
        WHERE UserId = @UserId";
}

