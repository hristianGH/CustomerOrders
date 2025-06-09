using CO.API.Data;
using CO.API.Data.Entities;
using CO.API.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CO.API.Test
{
    public class CustomerHandlerTests
    {
        private readonly ApiDbContext _dbContext;
        private readonly CustomerHandler _handler;
        private readonly ILogger<CustomerHandler> logger = new LoggerFactory().CreateLogger<CustomerHandler>();

        public CustomerHandlerTests()
        {
            DbContextOptions<ApiDbContext> options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;
            _dbContext = new ApiDbContext(options);
            _handler = new CustomerHandler(_dbContext, logger);
        }

        [Fact]
        public async Task GetCustomersAsync_ReturnsListOfCustomers()
        {
            List<Customer> mockCustomers =
            [
                new Customer {CompanyName = "testName2", CustomerID = "John Doe"},
                new Customer {CompanyName = "testName2", CustomerID = "Jane Smith"}
            ];
            _dbContext.AddRange(mockCustomers);
            await _dbContext.SaveChangesAsync();
            IEnumerable<Models.CustomersResponse> result = await _handler.GetAllCustomersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

    }
}
