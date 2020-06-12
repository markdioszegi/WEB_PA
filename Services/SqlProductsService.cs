using System;
using System.Collections.Generic;
using System.Data;
using PA.Models;

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
                Name = reader["name"] as string,
                Category = reader["category"] as string,
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

        public void AddProduct(ProductModel product)
        {
            using var command = _connection.CreateCommand();

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "name";
            nameParam.Value = product.Name;

            var categoryParam = command.CreateParameter();
            categoryParam.ParameterName = "category";
            categoryParam.Value = product.Category;

            var descriptionParam = command.CreateParameter();
            descriptionParam.ParameterName = "description";
            descriptionParam.Value = product.Description;

            var priceParam = command.CreateParameter();
            priceParam.ParameterName = "price";
            priceParam.Value = product.Price;

            var quantityParam = command.CreateParameter();
            quantityParam.ParameterName = "quantity";
            quantityParam.Value = product.Quantity;

            command.CommandText = @"INSERT INTO products (name, category, description, price, quantity) VALUES (@name, @category, @description, @price, @quantity)";
            command.Parameters.Add(categoryParam);
            command.Parameters.Add(nameParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(priceParam);
            command.Parameters.Add(quantityParam);

            HandleExecuteNonQuery(command);
        }
    }
}