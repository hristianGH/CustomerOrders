using CO.API.Controllers;
using CO.API.Data;
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
                .UseInMemoryDatabase(databaseName: "TestDatabase")
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
                new() { CompanyName = "testName", CustomerID= "John Doe" },
                new() { CompanyName = "testName2", CustomerID = "Jane Smith" }
            });
            await _dbContext.SaveChangesAsync();

            IActionResult result = await _controller.GetClients();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<CustomersResponse> returnedCustomers = Assert.IsType<List<CustomersResponse>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }
    }
}
