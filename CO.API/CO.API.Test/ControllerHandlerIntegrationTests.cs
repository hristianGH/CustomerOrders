using CO.API.Controllers;
using CO.API.Data;
using CO.API.Exceptions;
using CO.API.Handlers;
using CO.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CO.API.Test
{
    public class ControllerHandlerIntegrationTests
    {
        private readonly ApiDbContext _dbContext;
        private readonly CustomerHandler _handler;
        private readonly CustomersController _controller;
        private readonly ILogger<CustomersController> _logger = new LoggerFactory().CreateLogger<CustomersController>();
        private readonly ILogger<CustomerHandler> _loggerHandler = new LoggerFactory().CreateLogger<CustomerHandler>();

        public ControllerHandlerIntegrationTests()
        {
            DbContextOptions<ApiDbContext> options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new ApiDbContext(options);
            _handler = new CustomerHandler(_dbContext, _loggerHandler);
            _controller = new CustomersController(_logger, _handler);
        }

        [Fact]
        public async Task GetCustomers_IntegrationTest_ReturnsOkResult()
        {

            _dbContext.Customers.AddRange(new List<Data.Entities.Customer>
            {
                new() { CompanyName = "testName", CustomerID= "John Doee" },
                new() { CompanyName = "testName2", CustomerID = "Jane Smith" }
            });
            await _dbContext.SaveChangesAsync();

            IActionResult result = await _controller.GetClients();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<CustomersResponse> returnedCustomers = Assert.IsType<List<CustomersResponse>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomers_IntegrationTest_ReturnsEmptyList_WhenNoCustomersExist()
        {
            IActionResult result = await _controller.GetClients();
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<CustomersResponse> returnedCustomers = Assert.IsType<List<CustomersResponse>>(okResult.Value);
            Assert.Empty(returnedCustomers);
        }

        [Fact]
        public async Task GetClientById_ReturnsCustomer_WhenCustomerExists()
        {
            _dbContext.Customers.Add(new Data.Entities.Customer { CompanyName = "testName", CustomerID = "John Doe" });
            await _dbContext.SaveChangesAsync();

            IActionResult result = await _controller.GetClientById("John Doe");

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            CustomerDetailResponse returnedCustomer = Assert.IsType<CustomerDetailResponse>(okResult.Value);
            Assert.Equal("John Doe", returnedCustomer.CustomerID);
        }


        [Fact]
        public async Task GetClientOrdersById_ReturnsOrders_WhenOrdersExist()
        {
            _dbContext.Orders.AddRange(new List<Data.Entities.Order>
            {
                new() { OrderID = 1, CustomerID = "John Doe" },
                new() { OrderID = 2, CustomerID = "John Doe" }
            });
            _dbContext.Customers.Add(new Data.Entities.Customer { CompanyName = "testName", CustomerID = "John Doe" });

            await _dbContext.SaveChangesAsync();

            IActionResult result = await _controller.GetClientOrdersById("John Doe");

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<CustomerOrderResponse> returnedOrders = Assert.IsType<List<CustomerOrderResponse>>(okResult.Value);
            Assert.Equal(2, returnedOrders.Count);
        }

        [Fact]
        public async Task GetClientOrdersById_ReturnsEmptyList_WhenNoOrdersExist()
        {
            CustomerNotFoundException ex = await Assert.ThrowsAsync<CustomerNotFoundException>(async () => await _controller.GetClientOrdersById("NonExistentID"));
            Assert.Equal(("Customer with ID 'NonExistentID' not found."), ex.Message);

        }
    }
}
