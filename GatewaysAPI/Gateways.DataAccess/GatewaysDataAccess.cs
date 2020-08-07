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
        private static int DeviceIdSequence = 1;

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

        public Task<int> AddDeviceToGateway(string gatewaySerialNumber, Device newDevice)
        {
            newDevice.UniqueID = GetNewDeviceUniqueId();

            Gateway targetGateway = GatewaysDictionary[gatewaySerialNumber];
            if (targetGateway.Devices == null) 
            {
                targetGateway.Devices = new List<Device>();
            }

            targetGateway.Devices.Add(newDevice);
            return Task.FromResult(newDevice.UniqueID);
        }

        private int GetNewDeviceUniqueId() 
        {
            return DeviceIdSequence++;
        }

        public Task RemoveDeviceFromGateway(string gatewaySerialNumber, int deviceId)
        {
            Gateway targetGateway = GatewaysDictionary[gatewaySerialNumber];

            if (targetGateway.Devices != null) 
            {
                foreach (Device device in targetGateway.Devices) 
                {
                    if (device.UniqueID == deviceId) 
                    {
                        targetGateway.Devices.Remove(device);
                        break;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
