namespace PA
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        /* public Product(int id, string category, string name, string description, float price, int quantity)
        {
            Id = id;
            Category = category;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        } */

        public override string ToString()
        {
            return $"id: {Id}\ncategory: {Category}\nname: {Name}\ndescription: {0}\nprice: {Price}\nquantity: {Quantity}";
        }
    }
}