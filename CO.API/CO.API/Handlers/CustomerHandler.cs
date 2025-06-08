using CO.API.Data;
using CO.API.Exceptions;
using CO.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CO.API.Handlers
{
    public class CustomerHandler(ApiDbContext dbContext, ILogger<CustomerHandler> logger) : ICustomerHandler
    {
        private const string _discontinuedProducts = "Order contains discontinued products or insufficient stock.";

        public async Task<IEnumerable<CustomersResponse>> GetAllCustomersAsync()
        {
            try
            {
                logger.LogInformation("START: CustomerHandler. GetAllCustomersAsync()");
                return await dbContext.Customers
                    .Select(c => new CustomersResponse
                    {
                        CustomerID = c.CustomerID,
                        CompanyName = c.CompanyName,
                        NumberOfOrders = c.Orders.Count()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ERROR: CustomerHandler. GetAllCustomersAsync()");
                throw;
            }
            finally
            {
                logger.LogInformation("END: CustomerHandler. GetAllCustomersAsync()");
            }
        }

        public async Task<CustomerDetailResponse> GetCustomerAsync(string customerId)
        {
            try
            {
                logger.LogInformation($"START: CustomerHandler. GetCustomerAsync({customerId})");
                Data.Entities.Customer? customer = await dbContext.Customers.FindAsync(customerId);
                if (customer == null)
                {
                    throw new CustomerNotFoundException(customerId);
                }

                return new CustomerDetailResponse
                {
                    CustomerID = customer.CustomerID,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Address = customer.Address,
                    City = customer.City,
                    Region = customer.Region,
                    PostalCode = customer.PostalCode,
                    Country = customer.Country,
                    Phone = customer.Phone,
                    Fax = customer.Fax
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ERROR: CustomerHandler. GetCustomerAsync({customerId})");
                throw;
            }
            finally
            {
                logger.LogInformation($"END: CustomerHandler. GetCustomerAsync({customerId})");
            }
        }

        public async Task<IEnumerable<CustomerOrderResponse>> GetCustomerOrdersAsync(string customerId)
        {
            try
            {
                logger.LogInformation($"START: CustomerHandler. GetCustomerOrdersAsync({customerId})");

                if (!dbContext.Customers.Any(c => c.CustomerID == customerId))
                {
                    throw new CustomerNotFoundException(customerId);
                }

                List<CustomerOrderResponse> orders = await dbContext.Orders
                    .Where(o => o.CustomerID == customerId)
                    .Select(o => new CustomerOrderResponse
                    {
                        OrderID = o.OrderID,
                        OrderDate = o.OrderDate,
                        RequiredDate = o.RequiredDate,
                        ShippedDate = o.ShippedDate,
                        Total = o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)),
                        ProductCount = o.OrderDetails.Sum(od => od.Quantity),
                        Warning = o.OrderDetails.Any(od =>
                            od.Product!.Discontinued || od.Product.UnitsInStock < od.Product.UnitsOnOrder)
                            ? _discontinuedProducts
                            : null!
                    })
                    .ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ERROR: CustomerHandler. GetCustomerOrdersAsync({customerId})");
                throw;
            }
            finally
            {
                logger.LogInformation($"END: CustomerHandler. GetCustomerOrdersAsync({customerId})");
            }
        }

    }
}