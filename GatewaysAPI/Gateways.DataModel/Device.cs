using System;
using System.Collections.Generic;
using System.Text;

namespace Gateways.DataModel
{
    public class Device
    {
        public int UniqueID;
        public string Vendor;
        public DateTime DateCreated;
        private string status;
        public string Status 
        {
            get { return status; }
            set { 
                    if (value == "Online" || value == "Offline") 
                    {
                        status = value;
                    } 
            } 
        }
    }
}
