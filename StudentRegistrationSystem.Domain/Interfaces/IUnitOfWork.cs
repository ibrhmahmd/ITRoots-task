using System;
using System.Data;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    IUserRepository Users { get; }
    IStudentRepository Students { get; }
    IRegistrationRepository Registrations { get; }
    IPasswordResetTokenRepository PasswordResetTokens { get; }

    void BeginTransaction();
    void Commit();
    void Rollback();
}
