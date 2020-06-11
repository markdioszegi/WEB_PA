using System.Collections.Generic;
using System.Data;

namespace PA.Services
{
    public class PostgreSqlDBService : IDBService
    {
        private readonly IDbConnection _connection;

        public PostgreSqlDBService(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool Initialized
        {
            get
            {
                using var command = _connection.CreateCommand();
                command.CommandText = @"
                    SELECT EXISTS (
                        SELECT *
                        FROM information_schema.tables
                        WHERE table_name = 'comment'
                    )
                ";
                return (bool)command.ExecuteScalar();
            }
        }


    }
}