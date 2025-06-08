using CO.API.Models;

namespace CO.API.Handlers
{
    public interface ICustomerHandler
    {
        Task<IEnumerable<CustomersResponse>> GetAllCustomersAsync();

        Task<CustomerDetailResponse> GetCustomerAsync(string customerId);

        Task<IEnumerable<CustomerOrderResponse>> GetCustomerOrdersAsync(string customerId);
    }
}
