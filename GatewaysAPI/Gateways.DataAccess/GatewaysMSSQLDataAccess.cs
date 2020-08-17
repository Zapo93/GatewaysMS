using Gateways.DataAccess.DataModel;
using Gateways.DataModel;
using Gateways.Interfaces;
using Gateways.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.DataAccess
{
    public class GatewaysMSSQLDataAccess : IGatewaysDataAccess
    {
        private readonly string DBConnectionString;

        public GatewaysMSSQLDataAccess(string dbConnectionString) 
        {
            DBConnectionString = dbConnectionString;
        }

        public async Task AddGateway(Gateway newGateway)
        {
            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                SqlCommand command = CreateStoredProcedureCommand("add_gateway", connection);

                command.Parameters.AddWithValue("@SerialNumber", newGateway.SerialNumber);
                command.Parameters.AddWithValue("@Name", newGateway.Name);
                command.Parameters.AddWithValue("@IPv4", newGateway.IP);

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException error)
                {
                    if (error.Number == 2601) 
                    {
                        throw new GatewayAlreadyExistsException();
                    }
                    throw;
                }
            }
        }

        public async Task DeleteGatewayBySerialNumber(string serialNumber)
        {
            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                SqlCommand command = CreateStoredProcedureCommand("delete_gateway", connection);

                command.Parameters.AddWithValue("@GatewaySerialNumber", serialNumber);

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public async Task<int> AddDeviceToGateway(string gatewaySerialNumber, Device newDevice)
        {
            int deviceId = 0;

            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                SqlCommand command = CreateStoredProcedureCommand("add_device", connection);

                command.Parameters.AddWithValue("@GatewaySerialNumber", gatewaySerialNumber);
                command.Parameters.AddWithValue("@Vendor", newDevice.Vendor);
                command.Parameters.AddWithValue("@Status", newDevice.Status);
                command.Parameters.AddWithValue("@DateCreated", newDevice.DateCreated);

                SqlParameter deviceIdOutputParameter = new SqlParameter();
                deviceIdOutputParameter.ParameterName = "@DeviceId";
                deviceIdOutputParameter.SqlDbType = SqlDbType.Int;
                deviceIdOutputParameter.Direction = ParameterDirection.Output;

                command.Parameters.Add(deviceIdOutputParameter);

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    deviceId = (int)deviceIdOutputParameter.Value;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return deviceId;
        }

        public async Task RemoveDeviceFromGateway(string gatewaySerialNumber, int deviceId)
        {
            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                SqlCommand command = CreateStoredProcedureCommand("delete_device", connection);

                command.Parameters.AddWithValue("@DeviceId", deviceId);

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }


        public async Task SaveGatewayStatus(string gatewaySerialNumber, Gateway newStatus)
        {
            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    foreach (var device in newStatus.Devices)
                    {
                        if (device.Status != null)
                        {
                            SqlCommand command = CreateStoredProcedureCommand("set_device_status", connection);

                            command.Parameters.AddWithValue("@DeviceId", device.UniqueID);
                            command.Parameters.AddWithValue("@Status", device.Status);

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }         
            }
        }

        public async Task<Gateway[]> GetAllGateways()
        {
            Dictionary<string,Gateway> gatewaysDictionary = new Dictionary<string, Gateway>();

            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                SqlCommand command = CreateStoredProcedureCommand("get_all_gateways", connection);

                try
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await MapGatewaysResultReaderToDictionary(reader, gatewaysDictionary);
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return gatewaysDictionary.Values.ToArray();
        }

        public async Task<Gateway> GetGatewayBySerialNumber(string serialNumber)
        {
            Dictionary<string, Gateway> gatewaysDictionary = new Dictionary<string, Gateway>();

            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                SqlCommand command = CreateStoredProcedureCommand("get_gateway", connection);

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@SerialNumber";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = serialNumber;

                command.Parameters.Add(parameter);
                                
                try
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await MapGatewaysResultReaderToDictionary(reader, gatewaysDictionary);
                        }
                        else
                        {
                            return null;
                        }
                        reader.Close();
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return gatewaysDictionary.Values.ToArray()[0];
        }

        private SqlCommand CreateStoredProcedureCommand(string name, SqlConnection connection) 
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = name;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        private async Task MapGatewaysResultReaderToDictionary(SqlDataReader reader, Dictionary<string, Gateway> gatewaysDictionary) 
        {
            await FillGatewaysFromDataReader(gatewaysDictionary, reader);
            await reader.NextResultAsync();
            var allDevicesList = await GetDevicesFromDataReader(reader);
            MapDevicesToGateways(gatewaysDictionary, allDevicesList);
        }
        
        private async Task<Dictionary<string, Gateway>> FillGatewaysFromDataReader(Dictionary<string, Gateway> gatewaysDictionary, SqlDataReader reader)
        {
            while (await reader.ReadAsync())
            {
                var gateway = new Gateway();
                gateway.SerialNumber = reader.GetString(reader.GetOrdinal("serial_number"));
                gateway.Name = reader.GetString(reader.GetOrdinal("name"));
                gateway.IP = reader.GetString(reader.GetOrdinal("ipv4"));
                gatewaysDictionary.Add(gateway.SerialNumber, gateway);
            }

            return gatewaysDictionary;
        }

        private async Task<List<DeviceSQLModel>> GetDevicesFromDataReader(SqlDataReader reader)
        {
            var devicesList = new List<DeviceSQLModel>();

            while (await reader.ReadAsync())
            {
                var device = new DeviceSQLModel();
                device.UniqueID = reader.GetInt32(reader.GetOrdinal("id"));
                device.GatewaySerialNumber = reader.GetString(reader.GetOrdinal("serial_number"));
                device.Vendor = reader.GetString(reader.GetOrdinal("vendor"));
                device.DateCreated = reader.GetDateTime(reader.GetOrdinal("date_created"));
                if (!reader.IsDBNull(reader.GetOrdinal("status"))) 
                {
                    device.Status = reader.GetString(reader.GetOrdinal("status"));
                }
                devicesList.Add(device);
            }

            return devicesList;
        }

        private void MapDevicesToGateways(Dictionary<string,Gateway> gatewaysDictionary, List<DeviceSQLModel> devicesList) 
        {
            foreach (var deviceFromDb in devicesList) 
            {
                var actualDevice = new Device();
                actualDevice.UniqueID = deviceFromDb.UniqueID;
                actualDevice.Vendor = deviceFromDb.Vendor;
                actualDevice.DateCreated = deviceFromDb.DateCreated;
                actualDevice.Status = deviceFromDb.Status;

                gatewaysDictionary[deviceFromDb.GatewaySerialNumber].Devices.Add(actualDevice);
            }
        }
    }
}
