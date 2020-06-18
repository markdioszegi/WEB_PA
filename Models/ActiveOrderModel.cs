using System;
using System.Collections.Generic;

namespace PA.Models
{
    public class ActiveOrderModel
    {
        public Order Order { get; set; }
        public List<Product> Products { get; set; }
    }
}