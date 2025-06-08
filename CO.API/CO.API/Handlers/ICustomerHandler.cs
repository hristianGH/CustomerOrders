using CO.API.Models;

namespace CO.API.Handlers
{
    public interface ICustomerHandler
    {
        Task<IEnumerable<CustomersResponse>> GetAllCustomersAsync();
    }
}
