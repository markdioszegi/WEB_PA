using System;
using System.Collections.Generic;
using System.Data;

namespace PA.Services
{
    public class SqlProductsService : SqlBaseService, IProductsService
    {
        private readonly IDbConnection _connection;
        public SqlProductsService(IDbConnection connection)
        {
            _connection = connection;
        }

        static Product ToProduct(IDataReader reader)
        {
            return new Product
            {
                Id = (int)reader["id"],
                Category = reader["category"] as string,
                Name = reader["name"] as string,
                Description = reader["description"] as string,
                Price = (decimal)reader["price"],
                Quantity = (int)reader["quantity"],
            };
        }

        public List<Product> GetAll()
        {
            using var cmd = _connection.CreateCommand();
            string sql = "SELECT * FROM products";
            cmd.CommandText = sql;

            using var reader = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(ToProduct(reader));
            }
            return products;
        }

        public Product GetOne(int id)
        {
            using var command = _connection.CreateCommand();

            var param = command.CreateParameter();
            param.ParameterName = "id";
            param.Value = id;

            command.CommandText = "SELECT * FROM products WHERE id = @id";
            command.Parameters.Add(param);

            using var reader = command.ExecuteReader();
            reader.Read();
            return ToProduct(reader);
        }
    }
}