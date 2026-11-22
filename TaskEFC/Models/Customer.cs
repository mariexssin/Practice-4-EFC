namespace TaskEFC.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Discount { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}