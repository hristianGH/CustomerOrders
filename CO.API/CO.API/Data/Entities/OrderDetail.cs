﻿namespace CO.API.Data.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
