namespace StudentRegistrationSystem.Data.Queries;

public static class RegistrationQueries
{
    public const string GetById = @"
        SELECT RegistrationId AS Id, StudentId, CourseId, RegistrationDate, 
               Status, IsActive, CreatedAt, UpdatedAt
        FROM CourseRegistrations
        WHERE RegistrationId = @Id";

    public const string GetByStudentId = @"
        SELECT RegistrationId AS Id, StudentId, CourseId, RegistrationDate, 
               Status, IsActive, CreatedAt, UpdatedAt
        FROM CourseRegistrations
        WHERE StudentId = @StudentId
        ORDER BY RegistrationDate DESC";

    public const string GetActiveByStudentId = @"
        SELECT RegistrationId AS Id, StudentId, CourseId, RegistrationDate, 
               Status, IsActive, CreatedAt, UpdatedAt
        FROM CourseRegistrations
        WHERE StudentId = @StudentId AND IsActive = 1 AND Status = 'Registered'
        ORDER BY RegistrationDate DESC";

    public const string GetByCourseId = @"
        SELECT RegistrationId AS Id, StudentId, CourseId, RegistrationDate, 
               Status, IsActive, CreatedAt, UpdatedAt
        FROM CourseRegistrations
        WHERE CourseId = @CourseId
        ORDER BY RegistrationDate DESC";

    public const string IsRegistered = @"
        SELECT COUNT(1)
        FROM CourseRegistrations
        WHERE StudentId = @StudentId AND CourseId = @CourseId AND IsActive = 1";

    public const string Create = @"
        INSERT INTO CourseRegistrations (RegistrationId, StudentId, CourseId, 
                                        RegistrationDate, Status, IsActive, 
                                        CreatedAt, UpdatedAt)
        VALUES (@Id, @StudentId, @CourseId, @RegistrationDate, @Status, 
                @IsActive, @CreatedAt, @UpdatedAt)";

    public const string Update = @"
        UPDATE CourseRegistrations
        SET Status = @Status,
            IsActive = @IsActive,
            UpdatedAt = @UpdatedAt
        WHERE RegistrationId = @Id";

    public const string Delete = @"
        UPDATE CourseRegistrations
        SET IsActive = 0, Status = 'Dropped', UpdatedAt = @UpdatedAt
        WHERE RegistrationId = @Id";

    public const string GetRegistrationCount = @"
        SELECT COUNT(1)
        FROM CourseRegistrations
        WHERE CourseId = @CourseId AND IsActive = 1 AND Status = 'Registered'";
}
