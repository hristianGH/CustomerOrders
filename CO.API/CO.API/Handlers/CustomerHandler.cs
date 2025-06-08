using CO.API.Data;
using CO.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CO.API.Handlers
{
    public class CustomerHandler : ICustomerHandler
    {
        private readonly ApiDbContext _dbContext;
        public CustomerHandler(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<CustomersResponse>> GetAllCustomersAsync()
        {
            return await _dbContext.Customers
                .Select(c => new CustomersResponse
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName
                })
                .ToListAsync();
        }
    }
}
