using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gateways.DataModel
{
    public class Gateway
    {
        public string SerialNumber;
        public string Name;
        public string IP;
        public List<Device> Devices;

        public Gateway() 
        {
            Devices = new List<Device>();
        }

        public Device GetDeviceByUniqueId(int uniqueId) 
        {
            Device resultDevice = null;

            foreach (Device device in Devices) 
            {
                if (device.UniqueID == uniqueId) 
                {
                    resultDevice = device;
                    break;
                }
            }

            return resultDevice;
        }
    }
}
