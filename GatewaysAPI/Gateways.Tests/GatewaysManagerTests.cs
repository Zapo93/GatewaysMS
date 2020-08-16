using Gateways.DataAccess;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Interfaces.Exceptions;
using Gateways.Management;
using Gateways.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Tests
{
    [TestClass]
    public class GatewaysManagerTests
    {
        private readonly string DBConnectionString = "Server=localhost;Database=ms_gateways;Trusted_Connection=True;";

        [TestMethod]
        public async Task CreateAndDeleteGateway_ExistingSerialNumber_ThrowsError()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess(DBConnectionString);
            IGatewaysManager gatewaysManager = new GatewaysManager(dataAccess);

            Gateway newGateway = MockGateway.Create();

            await gatewaysManager.CreateGateway(newGateway);
            var gatewayFromDB = await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayAlreadyExistsException>(async () =>
            {
                await gatewaysManager.CreateGateway(newGateway);
            });

            await gatewaysManager.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayDoesNotExistException>(async () =>
            {
                await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);
            });

            Assert.AreEqual(newGateway.SerialNumber, gatewayFromDB.SerialNumber);
            Assert.AreEqual(newGateway.IP, gatewayFromDB.IP);
            Assert.AreEqual(newGateway.Name, gatewayFromDB.Name);
        }
    }
}
