namespace CO.API.Models
{
    public class CustomerOrderResponse
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal Total { get; set; }
        public int ProductCount { get; set; }
        public string Warning { get; set; }
    }
}
