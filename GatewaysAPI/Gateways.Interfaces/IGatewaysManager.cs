﻿using Gateways.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Interfaces
{
    public interface IGatewaysManager
    {
        Task CreateGateway(Gateway newGateway);
        Task DeleteGatewayBySerialNumber(string serialNumber);
        Task<Gateway[]> GetAllGateways();
    }
}