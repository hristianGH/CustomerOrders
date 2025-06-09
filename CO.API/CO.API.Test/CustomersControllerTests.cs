using CO.API.Controllers;
using CO.API.Handlers;
using CO.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CO.API.Test
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerHandler> _mockHandler;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockHandler = new Mock<ICustomerHandler>();
            _controller = new CustomersController(new LoggerFactory().CreateLogger<CustomersController>(), _mockHandler.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            List<CustomersResponse> mockCustomers =
            [
                new CustomersResponse { CustomerID = "test1", CompanyName = "John Doe" ,NumberOfOrders =3},
                new CustomersResponse { CustomerID = "test2", CompanyName = "Jane Smith", NumberOfOrders = 3 }
            ];
            _mockHandler.Setup(h => h.GetAllCustomersAsync()).ReturnsAsync(mockCustomers);

            // Act
            IActionResult result = await _controller.GetClients();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<CustomersResponse> returnedCustomers = Assert.IsType<List<CustomersResponse>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomers_ReturnsEmptyList_WhenHandlerReturnsEmptyList()
        {
            _mockHandler.Setup(h => h.GetAllCustomersAsync()).ReturnsAsync([]);

            IActionResult result = await _controller.GetClients();

            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
