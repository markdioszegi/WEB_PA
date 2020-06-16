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
            List<User> users = new List<User>();

            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM users";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(ToUser(reader));
            }
            return users;
        }

        public User GetOne(int id)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE id = @id";

            var idParam = command.CreateParameter();
            idParam.ParameterName = "id";
            idParam.Value = id;
            command.Parameters.Add(idParam);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ToUser(reader);
            }
            return null;
        }

        public User GetOne(string username)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE username = @username";

            var usernameParam = command.CreateParameter();
            usernameParam.ParameterName = "username";
            usernameParam.Value = username;
            command.Parameters.Add(usernameParam);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ToUser(reader);
            }
            return null;
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

            command.CommandText = @"INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
            command.Parameters.Add(usernameParam);
            command.Parameters.Add(passwordParam);
            command.Parameters.Add(emailParam);

            HandleExecuteNonQuery(command);
        }
    }
}