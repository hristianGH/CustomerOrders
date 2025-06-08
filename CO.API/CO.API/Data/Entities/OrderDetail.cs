namespace CO.API.Data.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; } // Composite PK
        public int ProductID { get; set; } // Composite PK

        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; } // Stored as float in Northwind

        public Order Order { get; set; }
        public Product Product { get; set; }
    }

}
