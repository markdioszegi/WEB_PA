using System;
using System.Collections.Generic;
using System.Data;
using PA.Models;

namespace PA.Services
{
    public class SqlOrdersService : SqlBaseService, IOrdersService
    {
        readonly IDbConnection _connection;
        public SqlOrdersService(IDbConnection connection)
        {
            _connection = connection;
        }

        static Order ToOrder(IDataReader reader)
        {
            return new Order
            {
                Id = (int)reader["id"],
                UserId = (int)reader["user_id"],
                OrderDate = (DateTime)reader["order_date"],
                FinalPrice = (decimal)reader["final_price"],
                Active = (bool)reader["active"],
            };
        }

        public static Product ToProduct(IDataReader reader)
        {
            return new Product
            {
                Id = (int)reader["id"],
                Name = reader["name"] as string,
                Category = reader["category"] as string,
                Description = reader["description"] as string,
                Price = (decimal)reader["price"],
                Stock = (int)reader["stock"],
                Quantity = (int)reader["quantity"],
            };
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

        public void RemoveActiveOrder(int userId)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            command.CommandText = @"UPDATE orders SET active = false WHERE user_id = @user_id AND active = true";
            command.Parameters.Add(userIdParam);

            HandleExecuteNonQuery(command);
        }

        public List<Order> GetAll(int userId)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            command.CommandText = "SELECT * FROM orders WHERE user_id = @user_id";
            command.Parameters.Add(userIdParam);

            using var reader = command.ExecuteReader();
            List<Order> orders = new List<Order>();
            while (reader.Read())
            {
                orders.Add(ToOrder(reader));
            }

            if (orders != null)
            {
                return orders;
            }
            return null;
        }

        public Order GetActive(int userId)
        {
            using var command = _connection.CreateCommand();

            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "user_id";
            userIdParam.Value = userId;

            command.CommandText = "SELECT * FROM orders WHERE user_id = @user_id and active = true";
            command.Parameters.Add(userIdParam);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ToOrder(reader);
            }
            return null;
        }

        public List<Product> GetProductsByOrderId(int orderId)
        {
            using var command = _connection.CreateCommand();

            var orderIdParam = command.CreateParameter();
            orderIdParam.ParameterName = "order_id";
            orderIdParam.Value = orderId;

            command.CommandText = "SELECT products.*, quantity FROM products " +
                                    "JOIN order_details ON id = order_details.product_id " +
                                    "WHERE order_id = @order_id";
            command.Parameters.Add(orderIdParam);

            using var reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(ToProduct(reader));
            }

            if (products != null)
            {
                return products;
            }
            return null;
        }

        public void AddProductToOrder(OrderDetail orderDetail)
        {
            using var command = _connection.CreateCommand();

            var orderIdParam = command.CreateParameter();
            orderIdParam.ParameterName = "order_id";
            orderIdParam.Value = orderDetail.OrderId;

            var productIdParam = command.CreateParameter();
            productIdParam.ParameterName = "product_id";
            productIdParam.Value = orderDetail.ProductId;

            var quantityParam = command.CreateParameter();
            quantityParam.ParameterName = "quantity";
            quantityParam.Value = orderDetail.Quantity;

            command.CommandText = "INSERT INTO order_details (order_id, product_id, quantity) " +
                                    "VALUES (@order_id, @product_id, @quantity)";
            command.Parameters.Add(orderIdParam);
            command.Parameters.Add(productIdParam);
            command.Parameters.Add(quantityParam);

            HandleExecuteNonQuery(command);
        }

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            using var command = _connection.CreateCommand();

            var orderIdParam = command.CreateParameter();
            orderIdParam.ParameterName = "order_id";
            orderIdParam.Value = orderId;

            var productIdParam = command.CreateParameter();
            productIdParam.ParameterName = "product_id";
            productIdParam.Value = productId;

            command.CommandText = "DELETE FROM order_details WHERE order_id = @order_id AND product_id = @product_id";
            command.Parameters.Add(orderIdParam);
            command.Parameters.Add(productIdParam);

            HandleExecuteNonQuery(command);
        }
    }
}