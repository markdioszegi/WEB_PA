using System.Collections.Generic;

namespace PA.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public List<Product> Products { get; set; }
    }
}