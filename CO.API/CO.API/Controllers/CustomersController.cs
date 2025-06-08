using CO.API.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace CO.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController(ILogger<CustomersController> logger, ICustomerHandler customerHandler) : ControllerBase
    {

        [HttpGet()]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                logger.LogInformation("START: CustomersController. GetClients()");
                return Ok(await customerHandler.GetAllCustomersAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ERROR: CustomersController. GetClients()");
                throw;
            }
            finally
            {
                logger.LogInformation("END: CustomersController. GetClients()");
            }
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetClientById(string id)
        {
            try
            {
                logger.LogInformation($"START: CustomersController. GetClientById({id})");
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Customer ID cannot be null or empty.");
                }

                return Ok(await customerHandler.GetCustomerAsync(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ERROR: CustomersController. GetClientById({id})");
                throw;
            }
            finally
            {
                logger.LogInformation($"END: CustomersController. GetClientById({id})");
            }
        }

        [HttpGet("customer/{id}/orders")]
        public async Task<IActionResult> GetClientOrdersById(string id)
        {
            try
            {
                logger.LogInformation($"START: CustomersController. GetClientById({id})");
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Customer ID cannot be null or empty.");
                }

                return Ok(await customerHandler.GetCustomerOrdersAsync(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"ERROR: CustomersController. GetClientById({id})");
                throw;
            }
            finally
            {
                logger.LogInformation($"END: CustomersController. GetClientById({id})");
            }
        }
    }
}
