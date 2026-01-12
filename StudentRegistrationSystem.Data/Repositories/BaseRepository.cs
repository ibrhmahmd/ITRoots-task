using System.Data;
using StudentRegistrationSystem.Data.Context;

namespace StudentRegistrationSystem.Data.Repositories;

public abstract class BaseRepository
{
    protected readonly IDbConnection _connection;
    protected readonly IDbTransaction? _transaction;

    protected BaseRepository(IDbConnection connection, IDbTransaction? transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    protected IDbConnection Connection => _connection;
}
