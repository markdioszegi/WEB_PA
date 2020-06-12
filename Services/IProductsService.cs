using System.Collections.Generic;
using PA.Models;

namespace PA.Services
{
    public interface IProductsService
    {
        List<Product> GetAll();
        Product GetOne(int id);
        void AddProduct(ProductModel product);
    }
}