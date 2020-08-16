using Gateways.DataModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Gateways.Tests.Mock
{
    internal class MockGateway
    {
        public static Gateway Create() 
        {
            Gateway newGateway = new Gateway();
            newGateway.IP = "192.168.22.10";
            newGateway.Name = "GatewayName";
            newGateway.SerialNumber = GetUniqueGatewaySerialNumber();

            return newGateway;
        }

        private static string GetUniqueGatewaySerialNumber()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Replace("+", string.Empty).Substring(0, 8);
        }
    }
}
