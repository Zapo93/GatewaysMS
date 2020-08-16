﻿using Gateways.DataModel;
using Gateways.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateways.Tests
{
    [TestClass]
    public class GatewayUnitTests
    {
        [TestMethod]
        public void Gateway_GetDeviceByUniqueId_GetDevice() 
        {
            Gateway gateway = MockGateway.Create();
            Device device = MockDevice.Create();
            device.UniqueID = 1;

            gateway.Devices.Add(device);

            var resultDevice = gateway.GetDeviceByUniqueId(device.UniqueID);

            Assert.AreEqual(device, resultDevice);
        }

        [TestMethod]
        public void Gateway_GetDeviceByUniqueId_NoSuchDevice()
        {
            Gateway gateway = MockGateway.Create();

            var resultDevice = gateway.GetDeviceByUniqueId(1);

            Assert.IsNull(resultDevice);
        }
    }
}
