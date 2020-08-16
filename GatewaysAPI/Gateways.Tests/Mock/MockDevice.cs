using Gateways.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateways.Tests.Mock
{
    internal class MockDevice
    {
        public static Device Create()
        {
            Device newDevice = new Device();
            newDevice.Vendor = "VendorName";
            newDevice.DateCreated = DateTime.UtcNow;
            newDevice.Status = "Online";

            return newDevice;
        }
    }
}
