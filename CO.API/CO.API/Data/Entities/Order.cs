namespace CO.API.Data.Entities
{
    public class Order
    {
        public int OrderID { get; set; } // PK
        public string CustomerID { get; set; } // FK to Customer
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}
