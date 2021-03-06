﻿using Gateways.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Interfaces
{
    public interface IGatewaysDataAccess
    {
        Task AddGateway(Gateway newGateway);
        Task<Gateway> GetGatewayBySerialNumber(string serialNumber);
        Task<Gateway[]> GetAllGateways();
        Task DeleteGatewayBySerialNumber(string serialNumber);
        Task<int> AddDeviceToGateway(string gatewaySerialNumber, Device newDevice);
        Task RemoveDeviceFromGateway(string gatewaySerialNumber, int deviceId);
        Task SaveGatewayStatus(string gatewaySerialNumber, Gateway newStatus);
    }
}
