using System;

namespace PA.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal FinalPrice { get; set; }
        public bool Active { get; set; }
    }
}