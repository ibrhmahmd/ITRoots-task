using System.Data;
using StudentRegistrationSystem.Data.Context;
using StudentRegistrationSystem.Domain.Interfaces;
using StudentRegistrationSystem.Domain.Interfaces.Repositories;

namespace StudentRegistrationSystem.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    private ICourseRepository? _courses;
    private IUserRepository? _users;
    private IStudentRepository? _students;
    private IRegistrationRepository? _registrations;
    private IPasswordResetTokenRepository? _passwordResetTokens;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private IDbConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                _connection = _connectionFactory.CreateConnection();
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
            }
            return _connection;
        }
    }

    // Lazy load repositories, passing the connection and transaction (which might be null)
    public ICourseRepository Courses => _courses ??= new CourseRepository(Connection, _transaction);
    public IUserRepository Users => _users ??= new UserRepository(Connection, _transaction);
    public IStudentRepository Students => _students ??= new StudentRepository(Connection, _transaction);
    public IRegistrationRepository Registrations => _registrations ??= new RegistrationRepository(Connection, _transaction);
    public IPasswordResetTokenRepository PasswordResetTokens => _passwordResetTokens ??= new PasswordResetTokenRepository(Connection, _transaction);

    public void BeginTransaction()
    {
        if (_transaction != null)
            return;

        _transaction = Connection.BeginTransaction();
        
        // Re-initializing repositories to ensure they pick up the new transaction
        _courses = null;
        _users = null;
        _students = null;
        _registrations = null;
        _passwordResetTokens = null;
    }

    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
            // Clear repos 
            _courses = null;
            _users = null;
            _students = null;
            _registrations = null;
            _passwordResetTokens = null;
        }
    }

    public void Rollback()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
            _courses = null;
            _users = null;
            _students = null;
            _registrations = null;
            _passwordResetTokens = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
