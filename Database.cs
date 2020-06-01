using System;
using System.Collections.Generic;
using Npgsql;

namespace PA
{
    public class Database
    {
        public List<User> Users = new List<User>();
        public readonly string ConnectionString;
        public NpgsqlConnection DatabaseConnection { get; private set; }

        public Database(string connectionString)
        {
            ConnectionString = connectionString;
            InitDB();
            DBTest();
            GetUsers();
        }

        private void InitDB()
        {
            DatabaseConnection = new NpgsqlConnection(ConnectionString);
            DatabaseConnection.Open();
        }

        private void DBTest()
        {
            var sqlstr = "select version()";

            using var cmd = new NpgsqlCommand(sqlstr, DatabaseConnection);
            var ver = cmd.ExecuteScalar().ToString();
            System.Console.WriteLine(ver);
        }

        //[HttpGet]
        private void GetUsers()
        {
            string sqlstr = "select * from users";


            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlstr, DatabaseConnection))
            {
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Int32.Parse(reader[0].ToString());
                    string username = reader[1].ToString();
                    string password = reader[2].ToString();
                    string email = reader[3].ToString();
                    string role = reader[4].ToString();

                    Users.Add(new User(id, username, password, email, role));
                }
            }

            //print debug
            foreach (var user in Users)
            {
                System.Console.WriteLine(user.ToString());
            }
        }
    }
}