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
    [ApiController]
    public class DevicesController : ControllerBase
    {
        IGatewaysManager GatewaysManager;

        public DevicesController(IGatewaysManager gatewaysManager)
        {
            GatewaysManager = gatewaysManager;
        }

        [HttpPost("api/gateways/{gatewaySerialNumber}/devices")]
        public async Task<ActionResult<int>> AddDeviceToGateway(string gatewaySerialNumber, [FromBody] Device newDevice)
        {
            try
            {
                return await GatewaysManager.AddDeviceToGateway(gatewaySerialNumber, newDevice);
            }
            catch (MaximumDevicesExceededException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Maximum device number in a gateway exceeded!");
            }
            catch (GatewayDoesNotExistException) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Gateway does not exist!");
            }
        }

        [HttpDelete("api/gateways/{gatewaySerialNumber}/devices/{deviceId}")]
        public async Task<ActionResult> RemoveDeviceFromGateway(string gatewaySerialNumber, int deviceId)
        {
            try
            {
                await GatewaysManager.RemoveDeviceFromGateway(gatewaySerialNumber, deviceId);
                return Ok();
            }
            catch (GatewayDoesNotExistException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Gateway does not exist!");
            }
        }
    }
}
