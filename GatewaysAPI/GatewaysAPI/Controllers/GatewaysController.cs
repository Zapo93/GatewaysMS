using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Management;
using Gateways.Management.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewaysAPI.Controllers
{
    [Route("api/gateways")]
    [ApiController]
    public class GatewaysController : ControllerBase
    {
        IGatewaysManager GatewaysManager;

        public GatewaysController() 
        {
            GatewaysManager = new GatewaysManager();
        }

        [HttpGet]
        public async Task<ActionResult<Gateway[]>> GetAllGateways() 
        {
            return await GatewaysManager.GetAllGateways();
        }

        [HttpPost]
        public async Task<ActionResult> CreateGateway([FromBody] Gateway newGateway)
        {
            try
            {
                await GatewaysManager.CreateGateway(newGateway);
                return Ok();
            }
            catch (GatewayAlreadyExistsException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Gateway already created!");
            }
            catch (InvalidIPv4Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid IPv4!");
            }
        }

        [HttpDelete("{serialNumber}")]
        public async Task<ActionResult> DeleteGateway(string serialNumber)
        {
            await GatewaysManager.DeleteGatewayBySerialNumber(serialNumber);
            return Ok();
        }
    }
}
