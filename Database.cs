using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

namespace PA
{
    public class Database
    {
        public List<User> Users = new List<User>();
        public List<Product> Products = new List<Product>();
        public readonly string ConnectionString;
        public NpgsqlConnection DatabaseConnection { get; private set; }

        public Database(string connectionString)
        {
            ConnectionString = connectionString;
            InitDB();
            DBTest();
            LoadUsers();
            //AddProduct();
            LoadProducts();
        }

        private void AddProduct()
        {
            string category = "cpu";
            string name = "PETABYTE RTX 3090 64TB GDDR9";
            string description = "very stronk";
            float price = 322.99647f;
            int quantity = 16;

            string sqlstr = "insert into products (category, name, description, price, quantity)" +
                            "values (@category, @name, @description, @price, @quantity)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlstr, DatabaseConnection))
            {
                cmd.Parameters.AddWithValue("category", category);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("description", description);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("quantity", quantity);
                cmd.ExecuteNonQuery();
            }
        }

        public void InitDB()
        {
            DatabaseConnection = new NpgsqlConnection(ConnectionString);
            DatabaseConnection.Open();
        }

        public void DBTest()
        {
            var sqlstr = "select version()";

            using var cmd = new NpgsqlCommand(sqlstr, DatabaseConnection);
            var ver = cmd.ExecuteScalar().ToString();
            System.Console.WriteLine(ver);
        }

        //[HttpGet]
        public void LoadUsers()
        {
            string sqlstr = "select * from users";

            NpgsqlCommand cmd = new NpgsqlCommand(sqlstr, DatabaseConnection);

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
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
            if (Users != null)
            {
                foreach (var user in Users)
                {
                    System.Console.WriteLine(user.ToString());
                }
            }
            else
            {
                System.Console.WriteLine("It's empty!");
            }
        }

        public void LoadProducts()
        {
            string sqlstr = "select * from products";

            NpgsqlCommand cmd = new NpgsqlCommand(sqlstr, DatabaseConnection);

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Int32.Parse(reader[0].ToString());
                    string category = reader[1].ToString();
                    string name = reader[2].ToString();
                    string description = reader[3].ToString();
                    float price = float.Parse(reader[4].ToString());
                    int quantity = Int32.Parse(reader[5].ToString());

                    Products.Add(new Product(id, category, name, description, price, quantity));
                }
            }

            //print debug
            if (Products != null)
            {
                foreach (var product in Products)
                {
                    System.Console.WriteLine(product.ToString());
                }
            }
            else
            {
                System.Console.WriteLine("It's empty!");
            }
        }
    }
}