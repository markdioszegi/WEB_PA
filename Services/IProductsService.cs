using System.Collections.Generic;

namespace PA.Services
{
    public interface IProductsService
    {
        List<Product> GetAll();
        Product GetOne(int id);
    }
}