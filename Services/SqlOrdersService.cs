using System.Data;

namespace PA.Services
{
    public class SqlOrdersService : SqlBaseService, IOrdersService
    {
        readonly IDbConnection _connection;
        public SqlOrdersService(IDbConnection connection)
        {
            _connection = connection;
        }
        public void CreateOrder(int userId)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            command.CommandText = @"INSERT INTO orders (user_id, order_date) VALUES (@user_id, now())";
            command.Parameters.Add(userIdParam);

            HandleExecuteNonQuery(command);
        }
    }
}