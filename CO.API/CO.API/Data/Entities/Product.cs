using System.ComponentModel.DataAnnotations;

namespace CO.API.Data.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }

}
