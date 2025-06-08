using System.ComponentModel.DataAnnotations;

namespace CO.API.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public string? CustomerID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}
