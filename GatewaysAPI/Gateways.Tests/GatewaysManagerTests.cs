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
        private readonly int MaxNumberOfDevicesInGateway = 10;

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

        [TestMethod]
        public async Task AddGatewayWithDevice_NewGateway_SuccessfullRead()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess(DBConnectionString);
            IGatewaysManager gatewaysManager = new GatewaysManager(dataAccess);

            Gateway newGateway = MockGateway.Create();
            Device newDevice = MockDevice.Create();

            newGateway.Devices.Add(newDevice);
            await gatewaysManager.CreateGateway(newGateway);
            var gatewayFromDB = await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);

            await gatewaysManager.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayDoesNotExistException>(async () =>
            {
                await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);
            });

            Assert.AreEqual(newGateway.SerialNumber, gatewayFromDB.SerialNumber);
            Assert.AreEqual(newGateway.IP, gatewayFromDB.IP);
            Assert.AreEqual(newGateway.Name, gatewayFromDB.Name);

            Assert.AreEqual(gatewayFromDB.Devices.Count, 1);
            Assert.AreEqual(newDevice.Vendor, gatewayFromDB.Devices[0].Vendor);
            Assert.AreEqual(newDevice.DateCreated.ToString(), gatewayFromDB.Devices[0].DateCreated.ToString());
            Assert.AreEqual(newDevice.Status, gatewayFromDB.Devices[0].Status);
        }

        [TestMethod]
        public async Task Add10DevicesToGateway_CreateGateway_Success()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess(DBConnectionString);
            IGatewaysManager gatewaysManager = new GatewaysManager(dataAccess);

            Gateway newGateway = MockGateway.Create();

            for (int i = 0; i < MaxNumberOfDevicesInGateway; i++)
            {
                Device newDevice = MockDevice.Create();
                newGateway.Devices.Add(newDevice);
            }

            await gatewaysManager.CreateGateway(newGateway);

            await gatewaysManager.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayDoesNotExistException>(async () =>
            {
                await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);
            });
        }

        [TestMethod]
        public async Task AddMoreThan10DevicesToGateway_CreateGateway_CorrespondingErrorThrown()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess(DBConnectionString);
            IGatewaysManager gatewaysManager = new GatewaysManager(dataAccess);

            Gateway newGateway = MockGateway.Create();

            for (int i=0; i<= MaxNumberOfDevicesInGateway; i++) 
            {
                Device newDevice = MockDevice.Create();
                newGateway.Devices.Add(newDevice);
            }

            await Assert.ThrowsExceptionAsync<MaximumDevicesExceededException>(async () =>
            {
                await gatewaysManager.CreateGateway(newGateway);
            });

            await gatewaysManager.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayDoesNotExistException>(async () =>
            {
                await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);
            });
        }

        [TestMethod]
        public async Task Add10DevicesToGateway_AddDevice_Success()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess(DBConnectionString);
            IGatewaysManager gatewaysManager = new GatewaysManager(dataAccess);

            Gateway newGateway = MockGateway.Create();

            for (int i = 0; i < MaxNumberOfDevicesInGateway - 1; i++)
            {
                Device newDevice = MockDevice.Create();
                newGateway.Devices.Add(newDevice);
            }

            await gatewaysManager.CreateGateway(newGateway);
            await gatewaysManager.AddDeviceToGateway(newGateway.SerialNumber, MockDevice.Create());
            await gatewaysManager.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayDoesNotExistException>(async () =>
            {
                await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);
            });
        }

        [TestMethod]
        public async Task AddMoreThan10DevicesToGateway_AddDevice_CorrespondingErrorThrown()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess(DBConnectionString);
            IGatewaysManager gatewaysManager = new GatewaysManager(dataAccess);

            Gateway newGateway = MockGateway.Create();

            for (int i = 0; i < MaxNumberOfDevicesInGateway; i++)
            {
                Device newDevice = MockDevice.Create();
                newGateway.Devices.Add(newDevice);
            }

            await gatewaysManager.CreateGateway(newGateway);

            await Assert.ThrowsExceptionAsync<MaximumDevicesExceededException>(async () =>
            {
                await gatewaysManager.AddDeviceToGateway(newGateway.SerialNumber,MockDevice.Create());
            });

            await gatewaysManager.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<GatewayDoesNotExistException>(async () =>
            {
                await gatewaysManager.GetGatewayStatus(newGateway.SerialNumber);
            });
        }


    }
}
