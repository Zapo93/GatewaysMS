using Gateways.DataAccess;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Management.Exceptions;
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

        public GatewaysManager() 
        {
            GatewayDataAcces = new GatewaysDataAccess();
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
    }
}
