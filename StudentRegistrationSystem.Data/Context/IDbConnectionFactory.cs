using System.Data;

namespace StudentRegistrationSystem.Data.Context;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
