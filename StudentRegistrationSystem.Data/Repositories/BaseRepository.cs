using System.Data;
using StudentRegistrationSystem.Data.Context;

namespace StudentRegistrationSystem.Data.Repositories;

public abstract class BaseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    protected IDbConnection CreateConnection()
    {
        return _connectionFactory.CreateConnection();
    }
}
