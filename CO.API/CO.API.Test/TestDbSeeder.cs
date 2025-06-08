using CO.API.Data;
using CO.API.Data.Entities;

namespace CO.API.Test
{
    public static class TestDbSeeder
    {
        public static void Seed(ApiDbContext db)
        {
            db.Customers.AddRange(
                new Customer
                {
                    CustomerID = "1",
                    CompanyName = "Test Company 1",
                    ContactName = "Alice",
                    Country = "USA",
                    Orders = new List<Order>
                    {
                        new Order { OrderID = 100, OrderDate = DateTime.UtcNow.AddDays(-10), Freight = 10.5m },
                        new Order { OrderID = 101, OrderDate = DateTime.UtcNow.AddDays(-5), Freight = 20.0m }
                    }
                },
                new Customer
                {
                    CustomerID = "2",
                    CompanyName = "Test Company 2",
                    ContactName = "Bob",
                    Country = "UK"
                }
            );
            db.SaveChanges();
        }
    }
}
