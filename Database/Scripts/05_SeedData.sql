USE IT_Roots_Task_Db;
GO

INSERT INTO Users (
        Username,
        Email,
        PasswordHash,
        Role,
        IsEmailVerified,
        CreatedAt
    )
VALUES (
        'admin',
        'admin@studentreg.com',
        '$2a$11$qR7Xg3YvN4u.I.zV7P6z5O.O.rG.M.P6Z5R7Xg3YvN4u.I.zV7P6z5O',
        2,
        1,
        GETDATE()
    );

INSERT INTO Courses (
        CourseCode, 
        CourseName, 
        Description, 
        Credits,
        Semester,
        SemesterYear,
        CreatedAt
    )
VALUES (
        'CS101',
        'Introduction to Computer Science',
        'Basic concepts of computer science.',
        3,
        1,
        2024,
        GETDATE()
    ),
    (
        'MAT101',
        'Calculus I',
        'Limits, derivatives, and integrals.',
        4,
        1,
        2024,
        GETDATE()
    ),
    (
        'ENG101',
        'English Composition',
        'Basic writing and communication skills.',
        2,
        1,
        2024,
        GETDATE()
    );
GO