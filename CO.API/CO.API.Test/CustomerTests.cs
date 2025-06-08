using CO.API.Data;
using CO.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace CO.API.Test
{
    public class CustomersControllerIntegrationTests
    {
        private readonly TestApiServer<CO.API.Startup> _testServer;

        public CustomersControllerIntegrationTests()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";
            _testServer = new TestApiServer<CO.API.Startup>(services =>
            {
                // Remove existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApiDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                // Register in-memory DB
                services.AddDbContext<ApiDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
                // Seed data after building provider
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
                db.Database.EnsureCreated();
                TestDbSeeder.Seed(db);
            });
        }

        [Fact]
        public async Task GetClients_ReturnsAllCustomers()
        {
            var response = await _testServer.GetAsync("/customers");
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            Assert.NotNull(customers);
            Assert.NotEmpty(customers);
        }

        [Fact]
        public async Task GetClientById_ReturnsCustomer()
        {
            var response = await _testServer.GetAsync("/customers/customer/1");
            response.EnsureSuccessStatusCode();
            var customer = await response.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(customer);
            Assert.Equal("1", customer.CustomerID);
        }

        [Fact]
        public async Task GetClientOrdersById_ReturnsOrders()
        {
            var response = await _testServer.GetAsync("/customers/customer/1/orders");
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
            Assert.NotNull(orders);
        }
    }
}