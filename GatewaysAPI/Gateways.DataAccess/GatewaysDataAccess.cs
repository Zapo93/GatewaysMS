using Gateways.DataModel;
using Gateways.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.DataAccess
{
    public class GatewaysDataAccess : IGatewaysDataAccess
    {
        private static Dictionary<string, Gateway> GatewaysDictionary = new Dictionary<string, Gateway>();

        public Task<Gateway> GetGatewayBySerialNumber(string serialNumber)
        {
            try
            {
                return Task.FromResult(GatewaysDictionary[serialNumber]);
            }
            catch (KeyNotFoundException)
            {
                return Task.FromResult<Gateway>(null);
            }
        }

        public Task AddGateway(Gateway newGateway) 
        {
            GatewaysDictionary[newGateway.SerialNumber] = newGateway;
            return Task.CompletedTask;
        }

        public Task<Gateway[]> GetAllGateways()
        {
            return Task.FromResult(GatewaysDictionary.Values.ToArray());
        }

        public Task DeleteGatewayBySerialNumber(string serialNumber)
        {
            GatewaysDictionary.Remove(serialNumber);
            return Task.CompletedTask;
        }
    }
}
