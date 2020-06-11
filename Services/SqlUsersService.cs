using System.Collections.Generic;
using System.Data;

namespace PA.Services
{
    public class SqlUsersService : SqlBaseService, IUsersService
    {
        private readonly IDbConnection _connection;

        public SqlUsersService(IDbConnection connection)
        {
            _connection = connection;
        }

        static User ToUser(IDataReader reader)
        {
            return new User
            {
                Id = (int)reader["id"],
                Username = reader["username"] as string,
                Password = reader["password"] as string,
                Email = reader["email"] as string,
                Role = reader["role"] as string
            };
        }

        public List<User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public User GetOne(int id)
        {
            throw new System.NotImplementedException();
        }

        public User Login(string username, string password)
        {
            using var command = _connection.CreateCommand();

            var usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "username";
            usernameParam.Value = username;

            var passwordParam = command.CreateParameter();
            passwordParam.ParameterName = "password";
            passwordParam.Value = password;

            command.CommandText = @"SELECT * FROM users WHERE username = @username AND password = @password";
            command.Parameters.Add(usernameParam);
            command.Parameters.Add(passwordParam);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ToUser(reader);
            }
            return null;
        }

        public void Register(string username, string password, string email)
        {
            using var command = _connection.CreateCommand();

            var usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "username";
            usernameParam.Value = username;

            var passwordParam = command.CreateParameter();
            passwordParam.ParameterName = "password";
            passwordParam.Value = password;

            var emailParam = command.CreateParameter();
            emailParam.ParameterName = "email";
            emailParam.Value = email;

            command.CommandText = @"INSERT INTO users (username, password, email, role) VALUES (@username, @password, @email, 'customer')";
            command.Parameters.Add(usernameParam);
            command.Parameters.Add(passwordParam);
            command.Parameters.Add(emailParam);

            HandleExecuteNonQuery(command);
        }
    }
}