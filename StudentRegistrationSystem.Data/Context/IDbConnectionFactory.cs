using System.Data;

namespace StudentRegistrationSystem.Data.Context;

/// <summary>
/// Factory interface for creating database connections
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a new database connection
    /// </summary>
    IDbConnection CreateConnection();
}
