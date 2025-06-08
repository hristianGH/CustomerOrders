using System.Net;

namespace CO.API.Exceptions
{
    public class CustomerNotFoundException(string customerId)
    : ApiException($"Customer with ID '{customerId}' not found.", (int)HttpStatusCode.NotFound)
    {
    }
}
