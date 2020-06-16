using System.Collections.Generic;
using PA.Models;

namespace PA.Services
{
    public interface IProductsService
    {
        List<Product> GetAll();
        Product GetOne(int id);
        Product GetOne(string name);
        void Add(ProductModel product);
        void Remove(int id);
        void Update(int id, ProductModel product);
    }
}