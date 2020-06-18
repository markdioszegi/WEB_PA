using System.Collections.Generic;
using PA.Models;

namespace PA.Services
{
    public interface IOrdersService
    {
        List<Order> GetAll(int userId);
        Order GetActive(int userId);
        List<Product> GetProductsByOrderId(int orderId);
        void CreateOrder(int userId);
        void RemoveActiveOrder(int userId);
        void AddProductToOrder(OrderDetail orderDetail);
        void RemoveProductFromOrder(int orderId, int productId);
    }
}