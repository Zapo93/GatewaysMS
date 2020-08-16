using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Interfaces.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewaysAPI.Controllers
{
    [Route("api/gateways/{gatewaySerialNumber}/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        IGatewaysManager GatewaysManager;

        public StatusController(IGatewaysManager gatewaysManager) 
        {
            GatewaysManager = gatewaysManager;
        }

        [HttpGet]
        public async Task<ActionResult<Gateway>> GetGatewayStatus(string gatewaySerialNumber)
        {
            try
            {
                return await GatewaysManager.GetGatewayStatus(gatewaySerialNumber);
            }
            catch (GatewayDoesNotExistException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Gateway does not exist!");
            }
            
        }

        [HttpPut]
        public async Task<ActionResult> Put(string gatewaySerialNumber, [FromBody] Gateway newStatus)
        {
            try
            {
                await GatewaysManager.UpdateGatewayStatus(gatewaySerialNumber, newStatus);
                return Ok();
            }
            catch (GatewayDoesNotExistException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Gateway does not exist!");
            }
        }
    }
}
