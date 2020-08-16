using Gateways.DataAccess;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Tests
{
    [TestClass]
    public class MSSQLDataAccessTests
    {
        [TestMethod]
        public async Task CreateAndDeleteGateway_NewSerialNumber_ReadFromTheDBSuccessfully() 
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess();
            Gateway newGateway = MockGateway.Create();

            await dataAccess.AddGateway(newGateway);
            var gatewayFromDB = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);
            await dataAccess.DeleteGatewayBySerialNumber(newGateway.SerialNumber);
            var deletionConfirmation = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);

            Assert.AreEqual(newGateway.SerialNumber, gatewayFromDB.SerialNumber);
            Assert.AreEqual(newGateway.IP, gatewayFromDB.IP);
            Assert.AreEqual(newGateway.Name, gatewayFromDB.Name);
            Assert.IsNull(deletionConfirmation);
        }

        [TestMethod]
        public async Task CreateAndDeleteGateway_ExistingSerialNumber_ThrowsError()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess();
            Gateway newGateway = MockGateway.Create();

            await dataAccess.AddGateway(newGateway);
            var gatewayFromDB = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);

            await Assert.ThrowsExceptionAsync<Exception>(async ()=> 
            {
                    await dataAccess.AddGateway(newGateway);
            });
            
            await dataAccess.DeleteGatewayBySerialNumber(newGateway.SerialNumber);
            var deletionConfirmation = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);

            Assert.AreEqual(newGateway.SerialNumber, gatewayFromDB.SerialNumber);
            Assert.AreEqual(newGateway.IP, gatewayFromDB.IP);
            Assert.AreEqual(newGateway.Name, gatewayFromDB.Name);
            
            Assert.IsNull(deletionConfirmation);
        }

        [TestMethod]
        public async Task CreateAndDeleteGateway_NewSerialNumbers_GetAllGatewaysSucccessfully()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess();
            Gateway newGateway = MockGateway.Create();
            Gateway gatewayFromDB = null;

            await dataAccess.AddGateway(newGateway);

            var gatewaysArrayFromDB = await dataAccess.GetAllGateways();
            for(int i=0; i<gatewaysArrayFromDB.Length;i++) 
            {
                if (gatewaysArrayFromDB[i].SerialNumber == newGateway.SerialNumber) 
                {
                    gatewayFromDB = gatewaysArrayFromDB[i];
                    break;
                }
            }
            await dataAccess.DeleteGatewayBySerialNumber(newGateway.SerialNumber);
            var deletionConfirmation = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);

            Assert.IsNotNull(gatewayFromDB);
            Assert.AreEqual(newGateway.SerialNumber, gatewayFromDB.SerialNumber);
            Assert.AreEqual(newGateway.IP, gatewayFromDB.IP);
            Assert.AreEqual(newGateway.Name, gatewayFromDB.Name);
            Assert.IsNull(deletionConfirmation);
        }

        [TestMethod]
        public async Task CreateAndDeleteDevice_NewDevice_ReadFromDBSuccessfully() 
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess();
            Gateway newGateway = MockGateway.Create();
            Device newDevice = MockDevice.Create();
            Device deviceFromDB = null;

            await dataAccess.AddGateway(newGateway);
            newDevice.UniqueID = await dataAccess.AddDeviceToGateway(newGateway.SerialNumber, newDevice);

            var gatewayFromDB = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);
            deviceFromDB = gatewayFromDB.GetDeviceByUniqueId(newDevice.UniqueID);

            await dataAccess.RemoveDeviceFromGateway(newGateway.SerialNumber, newDevice.UniqueID);

            gatewayFromDB = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);
            var deletionConfirmation = gatewayFromDB.GetDeviceByUniqueId(newDevice.UniqueID);

            await dataAccess.DeleteGatewayBySerialNumber(newGateway.SerialNumber);

            Assert.IsNotNull(deviceFromDB);
            Assert.AreEqual(newDevice.UniqueID, deviceFromDB.UniqueID);
            Assert.AreEqual(newDevice.Status, deviceFromDB.Status);
            Assert.AreEqual(newDevice.Vendor, deviceFromDB.Vendor);
            Assert.AreEqual(newDevice.DateCreated.ToString(), deviceFromDB.DateCreated.ToString());

            Assert.IsNull(deletionConfirmation);
        }

        [TestMethod]
        public async Task UpdateGatewayStatus_NewDevice_ReadFromDBSuccessfully()
        {
            IGatewaysDataAccess dataAccess = new GatewaysMSSQLDataAccess();
            Gateway newGateway = MockGateway.Create();
            Device newDevice = MockDevice.Create();
            Device deviceFromDB = null;

            newDevice.Status = "Offline";
            await dataAccess.AddGateway(newGateway);
            newDevice.UniqueID = await dataAccess.AddDeviceToGateway(newGateway.SerialNumber, newDevice);
            newGateway.Devices.Add(newDevice);

            var gatewayFromDB = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);
            deviceFromDB = gatewayFromDB.GetDeviceByUniqueId(newDevice.UniqueID);
            Assert.AreEqual(newDevice.UniqueID, deviceFromDB.UniqueID);
            Assert.AreEqual(newDevice.Status, deviceFromDB.Status);
            Assert.AreEqual(newDevice.Vendor, deviceFromDB.Vendor);
            Assert.AreEqual(newDevice.DateCreated.ToString(), deviceFromDB.DateCreated.ToString());

            newDevice.Status = "Online";
            await dataAccess.SaveGatewayStatus(newGateway.SerialNumber,newGateway);
            gatewayFromDB = await dataAccess.GetGatewayBySerialNumber(newGateway.SerialNumber);
            deviceFromDB = gatewayFromDB.GetDeviceByUniqueId(newDevice.UniqueID);
            Assert.AreEqual(newDevice.UniqueID, deviceFromDB.UniqueID);
            Assert.AreEqual(newDevice.Status, deviceFromDB.Status);
            Assert.AreEqual(newDevice.Vendor, deviceFromDB.Vendor);
            Assert.AreEqual(newDevice.DateCreated.ToString(), deviceFromDB.DateCreated.ToString());

            await dataAccess.DeleteGatewayBySerialNumber(newGateway.SerialNumber);            
        }
    }
}
