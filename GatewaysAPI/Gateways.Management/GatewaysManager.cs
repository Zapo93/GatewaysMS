using Gateways.DataAccess;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Management
{
    public class GatewaysManager : IGatewaysManager
    {
        IGatewaysDataAccess GatewayDataAcces;

        public GatewaysManager(IGatewaysDataAccess gatewaysDataAccess) 
        {
            GatewayDataAcces = gatewaysDataAccess;
        }
        
        public async Task CreateGateway(Gateway newGateway)
        {
            if (await GatewayDataAcces.GetGatewayBySerialNumber(newGateway.SerialNumber) != null) 
            {
                throw new GatewayAlreadyExistsException();
            }

            ValidateIPv4String(newGateway.IP);

            await GatewayDataAcces.AddGateway(newGateway);
        }

        private void ValidateIPv4String(string ipv4)
        {
            IPAddress address;
            if (!IPAddress.TryParse(ipv4, out address) || address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                throw new InvalidIPv4Exception();
            }
        }

        public Task<Gateway[]> GetAllGateways()
        {
            return GatewayDataAcces.GetAllGateways();
        }

        public Task DeleteGatewayBySerialNumber(string serialNumber)
        {
            return GatewayDataAcces.DeleteGatewayBySerialNumber(serialNumber);
        }

        public async Task<int> AddDeviceToGateway(string gatewaySerialNumber, Device newDevice)
        {
            Gateway targetGateway = await GetGatewayBySerialNumber(gatewaySerialNumber);
            
            if (targetGateway.Devices?.Count == 10) 
            {
                throw new MaximumDevicesExceededException();
            }

            newDevice.DateCreated = DateTime.UtcNow;

            return await GatewayDataAcces.AddDeviceToGateway(gatewaySerialNumber, newDevice);
        }

        public async Task RemoveDeviceFromGateway(string gatewaySerialNumber, int deviceId)
        {
            Gateway targetGateway = await GetGatewayBySerialNumber(gatewaySerialNumber);

            await GatewayDataAcces.RemoveDeviceFromGateway(gatewaySerialNumber, deviceId);
        }

        public Task<Gateway> GetGatewayStatus(string gatewaySerialNumber)
        {
            return GetGatewayBySerialNumber(gatewaySerialNumber);
        }

        public async Task UpdateGatewayStatus(string gatewaySerialNumber, Gateway newStatus)
        {
            await GetGatewayBySerialNumber(gatewaySerialNumber);
            await GatewayDataAcces.SaveGatewayStatus(gatewaySerialNumber, newStatus);
        }

        public async Task<Gateway> GetGatewayBySerialNumber(string gatewaySerialNumber) 
        {
            Gateway targetGateway = await GatewayDataAcces.GetGatewayBySerialNumber(gatewaySerialNumber);
            if (targetGateway == null)
            {
                throw new GatewayDoesNotExistException();
            }

            return targetGateway;
        }
    }
}
