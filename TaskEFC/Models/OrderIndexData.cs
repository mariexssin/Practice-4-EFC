namespace TaskEFC.Models
{
    public class OrderIndexData
    {
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();
        public IEnumerable<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}